namespace PickMeUp.Api.Account.Authentication.OAuth.Provider.Facebook
{
    // Deserialisation target for Facebook's /oauth/access_token response.

    public sealed class FacebookTokenResponse
    {
        public string AccessToken { get; init; } = string.Empty;
        public string TokenType { get; init; } = string.Empty;
        public int ExpiresIn { get; init; }
    }
}
