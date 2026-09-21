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

    // Minimal account entity used by the OAuth flow.

    public sealed class Account
    {
        public Guid Id { get; init; }
        public Email Email { get; init; } = null!;
        public Username Username { get; init; } = null!;
        public Avatar Avatar { get; init; } = null!;
    }

    // Creates a new account from an external OAuth identity.

    public sealed class AccountCreationService(IAccountRepository accounts)
    {
        private readonly IAccountRepository _accounts = accounts;

        // 1. Check if an account with this email already exists → return it.
        // 2. Otherwise, build Email, Username, Avatar value objects.
        // 3. Persist the new account via IAccountRepository.

        public Account CreateFromExternal(ExternalIdentity identity)
        {
            var existing = _accounts.FindByEmail(identity.Email);
            if (existing is not null)
                return existing;

            var email = Email.Create(identity.Email);
            var username = Username.Create(GenerateUsername(identity));
            var avatar = Avatar.Create("/avatars/default.png", AvatarTypeStatus.Default);

            var account = new Account
            {
                Id = Guid.NewGuid(),
                Email = email,
                Username = username,
                Avatar = avatar
            };

            return _accounts.Create(account);
        }

        private static string GenerateUsername(ExternalIdentity identity)
        {
            return identity.Name.Replace(" ", "").ToLowerInvariant();
        }
    }
}
