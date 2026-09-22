using Microsoft.AspNetCore.Identity;
using PickMeUp.Api.Account.Authentication;

namespace PickMeUp.Api.DoNotTouchFolder
{
    // User account entity extending ASP.NET Identity with domain fields.

    public class ApplicationUser : IdentityUser<Guid>
    {
        // ── Identity ──────────────────────────────────────────────

        public Guid Id { get; private set; } = Guid.NewGuid();
        public Email EmailValue { get; private set; } = null!;
        public string Username { get; private set; } = null!;
        public Password? PasswordValue { get; private set; } = null!;

        // ── Profile ───────────────────────────────────────────────

        public string? ShowUsername { get; private set; }

        // ── Status ────────────────────────────────────────────────

        public bool IsActive { get; private set; } = true;
        public bool IsLocked { get; private set; } = false;
        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

        // ── External Logins ───────────────────────────────────────

        public ICollection<IdentityUserLogin<Guid>> Logins { get; private set; } = [];

        // ── Constructors ──────────────────────────────────────────

        private ApplicationUser() { }

        public ApplicationUser(string username, Email email, Password password)
        {
            Id = Guid.NewGuid();
            Username = username;
            EmailValue = email;
            PasswordValue = password;
        }

        public ApplicationUser(string username, Email email)
        {
            Id = Guid.NewGuid();
            Username = username;
            EmailValue = email;
        }

        // ── Password Management ───────────────────────────────────

        public void ChangePassword(string currentPlainPassword, string newPlainPassword, IPasswordHasher<ApplicationUser> hasher)
        {
            if (PasswordValue is null)
                throw new InvalidOperationException("No password set on this account.");

            if (!PasswordValue.Verify(currentPlainPassword, hasher, this))
                throw new UnauthorizedAccessException("Current password is incorrect.");

            PasswordValue = Password.Create(newPlainPassword, hasher, this);
        }
    }
}
