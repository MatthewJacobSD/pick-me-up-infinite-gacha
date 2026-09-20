namespace PickMeUp.Api.Account.Authentication.Session
{
    // JWT configuration root. Holds issuer, audience, and token configs.
    // Maps to the "Jwt" section in appsettings.

    public sealed class Jwt
    {
        public const string SectionName = "Jwt";

        public string Issuer { get; }
        public string Audience { get; }

        public AccessTokenConfig AccessToken { get; }
        public RefreshTokenConfig RefreshToken { get; }
        public SessionConfig SessionConfig { get; }

        private Jwt(
            string issuer,
            string audience,
            AccessTokenConfig accessToken,
            RefreshTokenConfig refreshToken,
            SessionConfig sessionConfig)
        {
            if (string.IsNullOrWhiteSpace(issuer))
                throw new ArgumentException("Issuer cannot be empty");

            if (string.IsNullOrWhiteSpace(audience))
                throw new ArgumentException("Audience cannot be empty");

            Issuer = issuer;
            Audience = audience;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            SessionConfig = sessionConfig;
        }

        // Factory — validates all parts before creating the config object.
        public static Jwt Create(
            string issuer,
            string audience,
            AccessTokenConfig accessToken,
            RefreshTokenConfig refreshToken,
            SessionConfig sessionConfig)
        {
            if (accessToken is null)
                throw new ArgumentException("AccessToken cannot be null");

            if (refreshToken is null)
                throw new ArgumentException("RefreshToken cannot be null");

            return new Jwt(issuer, audience, accessToken, refreshToken, sessionConfig);
        }
    }
}
