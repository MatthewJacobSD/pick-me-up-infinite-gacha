using PickMeUp.Api.Account.Profile;
using static PickMeUp.Api.Account.Profile.Avatar;

namespace PickMeUp.Api.Account.Authentication.OAuth
{
    // Repository interface for account persistence.

    public interface IAccountRepository
    {
        Account? FindByEmail(string email);
        Account Create(Account account);
    }

    // Minimal account entity for MongoDB persistence.
    // Uses plain strings — value objects are for validation only, not storage.

    public sealed class Account
    {
        public Guid Id { get; init; }
        public string Email { get; init; } = string.Empty;
        public string Username { get; init; } = string.Empty;
        public string AvatarUrl { get; init; } = string.Empty;
    }

    // Creates a new account from an external OAuth identity.

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
            var username = Username.Create(GenerateUsername(identity));

            // 3. Persist as plain strings
            var account = new Account
            {
                Id = Guid.NewGuid(),
                Email = email.Address,
                Username = username.Value,
                AvatarUrl = "/avatars/default.png"
            };

            return _accounts.Create(account);
        }

        private static string GenerateUsername(ExternalIdentity identity)
        {
            return identity.Name.Replace(" ", "").ToLowerInvariant();
        }
    }
}
