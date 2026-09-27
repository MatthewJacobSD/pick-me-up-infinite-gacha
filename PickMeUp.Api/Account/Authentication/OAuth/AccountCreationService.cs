using System.Security.Cryptography;
using PickMeUp.Api.Account.Profile;
using static PickMeUp.Api.Account.Profile.Avatar;

namespace PickMeUp.Api.Account.Authentication.OAuth
{
    /**--------[Repository Interface]--------**/

    public interface IAccountRepository
    {
        Account? FindByEmail(string email);
        Account? FindByPublicCode(string publicCode);
        Account Create(Account account);
    }

    /**--------[Account Entity]--------**/

    /// <summary>
    /// Account entity for MongoDB persistence.
    /// Two IDs: Id (internal, server-only) and PublicCode (user-facing, MOE-XXXXXXXXXX).
    /// </summary>
    public sealed class Account
    {
        public Guid Id { get; init; }
        public string PublicCode { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Username { get; init; } = string.Empty;
        public string AvatarUrl { get; init; } = string.Empty;
    }

    /**--------[Account Creation]--------**/

    /// <summary>
    /// Creates a new account from an external OAuth identity.
    /// Returns the existing account if the email is already registered.
    /// </summary>
    public sealed class AccountCreationService(IAccountRepository accounts)
    {
        private readonly IAccountRepository _accounts = accounts;

        public Account CreateFromExternal(ExternalIdentity identity)
        {
            var existing = _accounts.FindByEmail(identity.Email);
            if (existing is not null)
                return existing;

            var email = Email.Create(identity.Email);
            var username = Username.Create(GenerateUsername());

            var account = new Account
            {
                Id = Guid.NewGuid(),
                PublicCode = GeneratePublicCode(),
                Email = email.Address,
                Username = username.Value,
                AvatarUrl = "/avatars/default.png"
            };

            return _accounts.Create(account);
        }

        /// <summary>Crypto-random username: "usr_" + 16 hex chars = 20 chars max.</summary>
        private static string GenerateUsername()
        {
            var bytes = RandomNumberGenerator.GetBytes(8);
            var hex = Convert.ToHexString(bytes).ToLowerInvariant();
            return $"usr_{hex}";
        }

        /// <summary>Public code: MOE- + 10 Crockford Base32 chars = 15 chars total.</summary>
        private static string GeneratePublicCode()
        {
            var bytes = RandomNumberGenerator.GetBytes(5);
            var code = CrockfordBase32.Encode(bytes);
            return $"MOE-{code}";
        }
    }

    /**--------[Crockford Base32]--------**/

    /// <summary>
    /// Crockford Base32 encoding (0-9, A-H, J-K, M-N, P-T, V-W, X-Y, Z).
    /// Excludes I, L, O, U to avoid confusion with 1, 1, 0, V.
    /// </summary>
    internal static class CrockfordBase32
    {
        private const string Alphabet = "0123456789ABCDEFGHJKMNPQRSTVWXYZ";

        public static string Encode(byte[] data)
        {
            var result = new System.Text.StringBuilder();
            int bits = 0;
            int value = 0;

            foreach (var b in data)
            {
                value = (value << 8) | b;
                bits += 8;
                while (bits >= 5)
                {
                    bits -= 5;
                    result.Append(Alphabet[(value >> bits) & 0x1F]);
                }
            }

            if (bits > 0)
                result.Append(Alphabet[(value << (5 - bits)) & 0x1F]);

            return result.ToString();
        }
    }
}
