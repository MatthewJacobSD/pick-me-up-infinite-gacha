using PickMeUp.Api.Account.Authentication.OAuth.Provider;

namespace PickMeUp.Api.Account.Authentication.OAuth
{
    // Builds the redirect URL that sends the user to the OAuth provider.

    public sealed class ExternalLoginService(OAuthProviderRegistry registry, OAuthStateValidator stateValidator)
    {
        private readonly OAuthProviderRegistry _registry = registry;
        private readonly OAuthStateValidator _stateValidator = stateValidator;

        // 1. Generate a CSRF state token via OAuthStateValidator.
        // 2. Look up the provider's client ID from the registry.
        // 3. Construct the full authorization URL with client_id, redirect_uri, scope, state.

        public string BuildRedirectUrl(OAuthProvider provider)
        {
            string state = _stateValidator.GenerateState();

            if (!_registry.HasProvider(provider))
                throw new InvalidOperationException($"OAuth provider {provider} is not configured");

            var callback = provider.CallbackPath();

            return provider switch
            {
                OAuthProvider.Google =>
                    $"https://accounts.google.com/o/oauth2/v2/auth?client_id={
                        ((GoogleProvider)_registry.GetProvider(provider)).ClientId
                        }&redirect_uri={callback}&response_type=code&scope=email profile&state={state}",

                OAuthProvider.Facebook =>
                    $"https://www.facebook.com/v18.0/dialog/oauth?client_id={
                        ((FacebookProvider)_registry.GetProvider(provider)).AppId
                        }&redirect_uri={callback}&response_type=code&scope=email public_profile&state={state}",

                _ => throw new InvalidOperationException("Unknown provider")
            };
        }
    }
}
