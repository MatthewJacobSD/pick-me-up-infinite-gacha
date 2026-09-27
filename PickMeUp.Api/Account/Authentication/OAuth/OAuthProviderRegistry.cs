namespace PickMeUp.Api.Account.Authentication.OAuth
{
    /// <summary>
    /// Checks whether a provider is configured and returns its config object.
    /// </summary>
    public sealed class OAuthProviderRegistry(OauthConfig config)
    {
        private readonly OauthConfig _config = config;

        public bool HasProvider(OAuthProvider provider)
        {
            return provider switch
            {
                OAuthProvider.Google => _config.Google is not null,
                OAuthProvider.Facebook => _config.Facebook is not null,
                _ => false
            };
        }

        /// <summary>
        /// Returns the raw config object for the given provider.
        /// Caller must cast to the concrete type (e.g. GoogleProvider).
        /// </summary>
        public object GetProvider(OAuthProvider provider)
        {
            return provider switch
            {
                OAuthProvider.Google => _config.Google,
                OAuthProvider.Facebook => _config.Facebook,
                _ => throw new InvalidOperationException("Unknown OAuth provider")
            };
        }
    }
}
