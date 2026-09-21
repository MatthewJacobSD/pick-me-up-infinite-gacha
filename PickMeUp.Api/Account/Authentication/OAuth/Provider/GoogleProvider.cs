namespace PickMeUp.Api.Account.Authentication.OAuth.Provider
{
    // Google OAuth provider config value object.

    public sealed class GoogleProvider
    {
        public string ClientId { get; private init; }
        public string ClientSecret { get; private init; }

        private GoogleProvider(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        public static GoogleProvider Create(string clientId, string clientSecret)
        {
            if (clientId is null)
                throw new ArgumentException("Google Client Id cannot be null");

            clientId = clientId.Trim();

            if (string.IsNullOrWhiteSpace(clientId))
                throw new ArgumentException("Google Client Id cannot be empty");

            if (clientSecret is null)
                throw new ArgumentException("Google Client Secret cannot be null");

            clientSecret = clientSecret.Trim();

            if (string.IsNullOrWhiteSpace(clientSecret))
                throw new ArgumentException("Google Client Secret cannot be empty");

            if (clientSecret.Length < 10)
                throw new ArgumentException("Google Client Secret looks invalid");

            return new GoogleProvider(clientId, clientSecret);
        }

        public override string ToString() => $"GoogleProvider(ClientId={ClientId})";
    }
}
