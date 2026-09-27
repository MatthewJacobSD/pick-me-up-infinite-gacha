using System.Text.Json.Serialization;

namespace PickMeUp.Api.Account.Authentication.OAuth.Provider.Facebook
{
    /// <summary>
    /// Deserialisation target for Facebook's /oauth/access_token endpoint response.
    /// </summary>
    public sealed class FacebookTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; } = string.Empty;

        [JsonPropertyName("token_type")]
        public string TokenType { get; init; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; init; }
    }
}
