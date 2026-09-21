namespace PickMeUp.Api.Account.Authentication.OAuth.Provider.Google
{
    // Deserialisation target for Google's /token response.

    public sealed class GoogleTokenResponse
    {
        public string AccessToken { get; init; } = string.Empty;
        public int ExpiresIn { get; init; }
        public string RefreshToken { get; init; } = string.Empty;
        public string Scope { get; init; } = string.Empty;
        public string TokenType { get; init; } = string.Empty;
    }
}
