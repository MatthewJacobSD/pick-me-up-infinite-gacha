using System.Net.Http.Json;
using System.Text.Json;
using PickMeUp.Api.Account.Authentication.OAuth.Provider;
using PickMeUp.Api.Account.Authentication.OAuth.Provider.Facebook;
using PickMeUp.Api.Account.Authentication.OAuth.Provider.Google;

namespace PickMeUp.Api.Account.Authentication.OAuth
{
    /// <summary>
    /// Handles the OAuth callback from Google/Facebook.
    /// Exchanges the authorization code for an access token, then fetches user info.
    /// State validation is done by the controller — this handler assumes state is already validated.
    /// </summary>
    public sealed class OAuthCallbackHandler(OAuthProviderRegistry registry, HttpClient http)
    {
        private readonly OAuthProviderRegistry _registry = registry;
        private readonly HttpClient _http = http;

        /// <summary>
        /// Exchanges the authorization code for user identity via the appropriate provider.
        /// </summary>
        public async Task<ExternalIdentity> HandleAsync(OAuthProvider provider, string code)
        {
            if (!_registry.HasProvider(provider))
                throw new InvalidOperationException("Provider not configured");

            return provider switch
            {
                OAuthProvider.Google => await HandleGoogleAsync(code),
                OAuthProvider.Facebook => await HandleFacebookAsync(code),
                _ => throw new InvalidOperationException("Unknown provider")
            };
        }

        /**--------[Google]--------**/

        private async Task<ExternalIdentity> HandleGoogleAsync(string code)
        {
            var google = (GoogleProvider)_registry.GetProvider(OAuthProvider.Google);

            var tokenResponse = await _http.PostAsync(
                "https://oauth2.googleapis.com/token",
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["client_id"] = google.ClientId,
                    ["client_secret"] = google.ClientSecret,
                    ["code"] = code,
                    ["grant_type"] = "authorization_code",
                    ["redirect_uri"] = OAuthProvider.Google.CallbackPath()
                })
            );

            var responseContent = await tokenResponse.Content.ReadAsStringAsync();

            if (!tokenResponse.IsSuccessStatusCode)
                throw new OAuthHttpException(OAuthProvider.Google, (int)tokenResponse.StatusCode,
                    $"Google token exchange failed: {responseContent}");

            var token = JsonSerializer.Deserialize<GoogleTokenResponse>(responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new OAuthTokenException(OAuthProvider.Google, "Google token response was null");

            var userInfo = await _http.GetFromJsonAsync<GoogleUserInfo>(
                $"https://www.googleapis.com/oauth2/v2/userinfo?access_token={token.AccessToken}")
                ?? throw new OAuthUserInfoException(OAuthProvider.Google, "Google user info was null");

            return new ExternalIdentity(
                Provider: OAuthProvider.Google,
                Email: userInfo.Email,
                Name: userInfo.Name,
                ExternalId: userInfo.Id
            );
        }

        /**--------[Facebook]--------**/

        private async Task<ExternalIdentity> HandleFacebookAsync(string code)
        {
            var facebook = (FacebookProvider)_registry.GetProvider(OAuthProvider.Facebook);
            var redirectUri = Uri.EscapeDataString(OAuthProvider.Facebook.CallbackPath());

            var tokenResponse = await _http.GetAsync(
                $"https://graph.facebook.com/v18.0/oauth/access_token?client_id={facebook.AppId}&client_secret={facebook.AppSecret}&code={code}&redirect_uri={redirectUri}");

            var responseContent = await tokenResponse.Content.ReadAsStringAsync();

            if (!tokenResponse.IsSuccessStatusCode)
                throw new OAuthHttpException(OAuthProvider.Facebook, (int)tokenResponse.StatusCode,
                    $"Facebook token exchange failed: {responseContent}");

            var token = JsonSerializer.Deserialize<FacebookTokenResponse>(responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new OAuthTokenException(OAuthProvider.Facebook, "Facebook token response was null");

            var userInfo = await _http.GetFromJsonAsync<FacebookUserInfo>(
                $"https://graph.facebook.com/me?fields=id,name,email&access_token={token.AccessToken}")
                ?? throw new OAuthUserInfoException(OAuthProvider.Facebook, "Facebook user info was null");

            return new ExternalIdentity(
                Provider: OAuthProvider.Facebook,
                Email: userInfo.Email,
                Name: userInfo.Name,
                ExternalId: userInfo.Id
            );
        }
    }
}
