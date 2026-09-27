using System.Text.Json.Serialization;

namespace PickMeUp.Api.Account.Authentication.OAuth.Provider.Google
{
    /// <summary>
    /// Deserialisation target for Google's /token endpoint response.
    /// </summary>
    public sealed class GoogleTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; init; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; init; } = string.Empty;

        [JsonPropertyName("scope")]
        public string Scope { get; init; } = string.Empty;

        [JsonPropertyName("token_type")]
        public string TokenType { get; init; } = string.Empty;
    }
}
