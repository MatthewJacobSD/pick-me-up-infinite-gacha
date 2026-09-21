using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace PickMeUp.Api.Account.Authentication
{
    // Value object wrapping a hashed password with policy enforcement.

    public sealed partial class Password
    {
        private readonly string _hash;
        public string Hash => _hash;

        private Password(string hash)
        {
            _hash = hash;
        }

        // ── Factory ───────────────────────────────────────────────

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

        [GeneratedRegex(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{12,}$"
        )]
        private static partial Regex MyRegex();
    }
}
