namespace PickMeUp.Api.Account.Authentication.Session
{
    /// <summary>
    /// JWT configuration root mapping to the "Jwt" section in appsettings.
    /// </summary>
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

        /// <summary>
        /// Factory that validates all sub-configs are present.
        /// </summary>
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
