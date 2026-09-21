using PickMeUp.Api.Account.Authentication.OAuth.Provider;
using PickMeUp.Api.Account.Authentication.OAuth.Provider.Facebook;
using PickMeUp.Api.Account.Authentication.OAuth.Provider.Google;

namespace PickMeUp.Api.Account.Authentication.OAuth
{
    // Handles the OAuth callback from Google/Facebook.

    public sealed class OAuthCallbackHandler(OAuthProviderRegistry registry, OAuthStateValidator stateValidator, HttpClient http)
    {
        private readonly OAuthProviderRegistry _registry = registry;
        private readonly OAuthStateValidator _stateValidator = stateValidator;
        private readonly HttpClient _http = http;

        // 1. Validate the CSRF state token.
        // 2. Exchange the authorization code for an access token.
        // 3. Fetch user info (email, name, external ID) from the provider.
        // 4. Return a normalised ExternalIdentity.

        public async Task<ExternalIdentity> HandleAsync(
            OAuthProvider provider,
            string code,
            string state)
        {
            if (!_stateValidator.ValidateState(state))
                throw new OAuthException(provider, "Invalid OAuth state (possible CSRF attack");

            if (!_registry.HasProvider(provider))
                throw new InvalidOperationException("Provider not configured");

            return provider switch
            {
                OAuthProvider.Google => await HandleGoogleAsync(code),
                OAuthProvider.Facebook => await HandleFacebookAsync(code),
                _ => throw new InvalidOperationException("Unknown provider")
            };
        }

        // ── Google ────────────────────────────────────────────────

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

            var token = await tokenResponse.Content.ReadFromJsonAsync<GoogleTokenResponse>();

            var userInfo = await _http.GetFromJsonAsync<GoogleUserInfo>(
                $"https://www.googleapis.com/oauth2/v2/userinfo?access_token={token!.AccessToken}")
                ?? throw new OAuthUserInfoException(OAuthProvider.Google, "Google user info was null");

            return new ExternalIdentity(
                Provider: OAuthProvider.Google,
                Email: userInfo.Email,
                Name: userInfo.Name,
                ExternalId: userInfo.Id
            );
        }

        // ── Facebook ──────────────────────────────────────────────

        private async Task<ExternalIdentity> HandleFacebookAsync(string code)
        {
            var facebook = (FacebookProvider)_registry.GetProvider(OAuthProvider.Facebook);

            var token = await _http.GetFromJsonAsync<FacebookTokenResponse>(
                $"https://graph.facebook.com/v18.0/oauth/access_token?client_id={facebook.AppId}&client_secret={facebook.AppSecret}&code={code}&redirect_uri={OAuthProvider.Facebook.CallbackPath()}");

            var userInfo = await _http.GetFromJsonAsync<FacebookUserInfo>(
                $"https://graph.facebook.com/me?fields=id,name,email&access_token={token!.AccessToken}")
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
