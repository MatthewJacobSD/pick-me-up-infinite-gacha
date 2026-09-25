using System.Text.RegularExpressions;

namespace PickMeUp.Api.Account.Profile
{
    // Username value object (3–20 chars, lowercase alphanumeric with underscores/hyphens).

    public sealed partial class Username
    {
        public string Value { get; set; } = string.Empty;

        // Parameterless constructor for MongoDB deserialization.
        private Username() { }

        private Username(string value)
        {
            Value = value;
        }

        public static Username Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Username cannot be empty");

            value = value.Trim().ToLowerInvariant();

            if (value.Length < 3)
                throw new ArgumentException("Username needs to be three character long to be accepted");

            if (value.Length > 20)
                throw new ArgumentException("Username is too long, use a shorter username");

            if (!MyRegex().IsMatch(value))
                throw new ArgumentException("Username contains invalid characters");

            return new Username(value);
        }

        // Allows: lowercase letters, digits, underscore, hyphen.
        [GeneratedRegex(@"^[a-z0-9_-]+$")]
        private static partial Regex MyRegex();
    }
}
