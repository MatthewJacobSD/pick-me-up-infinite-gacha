namespace PickMeUp.Api.Account.Authentication
{
    // Value object for a validated email address.
    // Normalises to lowercase, trims whitespace, and detects the provider domain.

    public sealed class Email : IEquatable<Email>
    {
        public string Address { get; }
        public EmailProvider Provider { get; }

        private Email(string address, EmailProvider provider)
        {
            Address = address;
            Provider = provider;
        }

        // Factory — validates, normalises, and detects provider.
        public static Email Create(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Email field cannot be empty");

            address = address.Trim().ToLowerInvariant();

            if (!IsValidEmail(address))
                throw new ArgumentException($"Invalid email: {address}");

            return new Email(address, DetectProvider(address));
        }

        // ── Equality ──────────────────────────────────────────────

        public override string ToString() => Address;

        public bool Equals(Email? other)
        {
            if (other is null) return false;
            return string.Equals(Address, other.Address, StringComparison.Ordinal);
        }

        public override bool Equals(object? obj) => Equals(obj as Email);

        public override int GetHashCode() => Address.GetHashCode(StringComparison.Ordinal);

        public static bool operator ==(Email? left, Email? right) => Equals(left, right);
        public static bool operator !=(Email? left, Email? right) => !Equals(left, right);

        // ── Validation ────────────────────────────────────────────

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // ── Provider Detection ────────────────────────────────────
        // Maps domain to EmailProvider for display/labelling only.
        // Actual OAuth sign-in is handled separately.

        private static EmailProvider DetectProvider(string email)
        {
            var domain = email.Split('@')[1];

            return domain switch
            {
                "moebius-order" => EmailProvider.MoebiusOrder,
                "gmail.com" or "googlemail.com" => EmailProvider.Google,
                "facebook.com" => EmailProvider.Facebook,
                _ => EmailProvider.Unknown
            };
        }
    }

    // Categorises an email by its domain for display purposes.
    // Does not control which OAuth providers are available for sign-in.

    public enum EmailProvider
    {
        Unknown = 0,
        MoebiusOrder = 1,
        Google = 2,
        Facebook = 3
    }
}
