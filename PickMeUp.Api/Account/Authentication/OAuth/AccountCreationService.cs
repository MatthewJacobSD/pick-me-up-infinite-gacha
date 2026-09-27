using System.Security.Cryptography;
using PickMeUp.Api.Account.Profile;
using static PickMeUp.Api.Account.Profile.Avatar;

namespace PickMeUp.Api.Account.Authentication.OAuth
{
    public interface IAccountRepository
    {
        Account? FindByEmail(string email);
        Account? FindByPersonalId(string personalId);
        Account Create(Account account);
    }

    // Account entity for MongoDB persistence.
    // Two IDs: AccountId (internal, server-only) + PersonalId (user-facing, crypto-generated).

    public sealed class Account
    {
        public Guid Id { get; init; }
        public string PersonalId { get; init; } = string.Empty; // PGMU-XXXXXXXX (user-facing)
        public string Email { get; init; } = string.Empty;
        public string Username { get; init; } = string.Empty;
        public string AvatarUrl { get; init; } = string.Empty;
    }

    public sealed class AccountCreationService(IAccountRepository accounts)
    {
        private readonly IAccountRepository _accounts = accounts;

        public Account CreateFromExternal(ExternalIdentity identity)
        {
            // 1. Check if account already exists
            var existing = _accounts.FindByEmail(identity.Email);
            if (existing is not null)
                return existing;

            // 2. Validate with value objects
            var email = Email.Create(identity.Email);

            // 3. Generate crypto-random username (max 20 chars)
            var username = Username.Create(GenerateUsername());

            // 4. Create account with personal ID
            var account = new Account
            {
                Id = Guid.NewGuid(),
                PersonalId = GeneratePersonalId(),
                Email = email.Address,
                Username = username.Value,
                AvatarUrl = "/avatars/default.png"
            };

            return _accounts.Create(account);
        }

        // Crypto-random username: "usr_" + 16 hex chars = 20 chars max
        private static string GenerateUsername()
        {
            var bytes = RandomNumberGenerator.GetBytes(8); // 16 hex chars
            var hex = Convert.ToHexString(bytes).ToLowerInvariant();
            return $"usr_{hex}"; // 4 + 16 = 20 chars
        }

        // Personal ID: "PGMU-" + 12 hex chars = 17 chars total
        private static string GeneratePersonalId()
        {
            var bytes = RandomNumberGenerator.GetBytes(6); // 12 hex chars
            var hex = Convert.ToHexString(bytes).ToUpperInvariant();
            return $"PGMU-{hex}"; // 5 + 12 = 17 chars
        }
    }
}
