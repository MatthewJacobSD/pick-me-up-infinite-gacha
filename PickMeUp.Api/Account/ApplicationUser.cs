using Microsoft.AspNetCore.Identity;
using PickMeUp.Api.Account.Authentication;

namespace PickMeUp.Api.Account
{
    // User account entity.
    // Extends ASP.NET Identity with domain-specific fields (email value object,
    // password value object, display name, account status).

    public class ApplicationUser
    {
        // ── Identity ──────────────────────────────────────────────

        public Guid Id { get; private set; } = Guid.NewGuid();
        public Email EmailValue { get; private set; } = null!;
        public string Username { get; private set; } = null!;
        public Password? PasswordValue { get; private set; } = null!;

        // ── Profile ───────────────────────────────────────────────

        // Optional public display name (distinct from Username).
        public string? ShowUsername { get; private set; }

        // ── Status ────────────────────────────────────────────────

        public bool IsActive { get; private set; } = true;
        public bool IsLocked { get; private set; } = false;
        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

        // ── External Logins ───────────────────────────────────────
        // Linked OAuth logins (Google, Facebook). Managed by ASP.NET Identity.

        public ICollection<IdentityUserLogin<Guid>> Logins { get; private set; } = [];

        // ── Constructors ──────────────────────────────────────────

        // Private constructor for EF Core materialisation.
        private ApplicationUser() { }

        // Local account (email + password).
        public ApplicationUser(string username, Email email, Password password)
        {
            Id = Guid.NewGuid();
            Username = username;
            EmailValue = email;
            PasswordValue = password;
        }

        // OAuth account (no local password).
        public ApplicationUser(string username, Email email)
        {
            Id = Guid.NewGuid();
            Username = username;
            EmailValue = email;
        }

        // ── Password Management ───────────────────────────────────

        // Changes password after verifying the current one.
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
