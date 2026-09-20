using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace PickMeUp.Api.Account.Authentication
{
    // Value object wrapping a hashed password.
    // Enforces policy (12+ chars, mixed case, digit, symbol) on creation.
    // Delegates verification to the ASP.NET Identity password hasher.

    public sealed partial class Password
    {
        private readonly string _hash;
        public string Hash => _hash;

        private Password(string hash)
        {
            _hash = hash;
        }

        // ── Factory ───────────────────────────────────────────────

        // Hashes a new plaintext password after validating it against policy.
        public static Password Create(string plainPassword, IPasswordHasher<ApplicationUser> hasher, ApplicationUser user)
        {
            if (string.IsNullOrWhiteSpace(plainPassword))
                throw new ArgumentException(
                    "Password field cannot be empty",
                    nameof(plainPassword)
                );

            if (!MyRegex().IsMatch(plainPassword))
                throw new ArgumentException(
                    "Password should contain at least one uppercase letter, " +
                    "one lowercase letter, one number and one symbol",
                    nameof(plainPassword)
                );

            string hash = hasher.HashPassword(user, plainPassword);
            return new Password(hash);
        }

        // Reconstitutes from an existing hash (e.g. loaded from the database).
        public static Password FromHash(string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
                throw new ArgumentException(
                    "Password hash is required and cannot be empty",
                    nameof(hash)
                );

            return new Password(hash);
        }

        // ── Verification ──────────────────────────────────────────

        // Verifies a plaintext password against the stored hash.
        public bool Verify(string plainPassword, IPasswordHasher<ApplicationUser> hasher, ApplicationUser user)
        {
            PasswordVerificationResult result = hasher.VerifyHashedPassword(
                user,
                _hash,
                plainPassword
            );

            return result != PasswordVerificationResult.Failed;
        }

        // ── Policy Regex ──────────────────────────────────────────
        // Requires: 1 lowercase, 1 uppercase, 1 digit, 1 symbol, 12+ chars.

        [GeneratedRegex(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{12,}$"
        )]
        private static partial Regex MyRegex();
    }
}
