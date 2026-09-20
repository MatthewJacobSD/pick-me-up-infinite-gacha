namespace PickMeUp.Api.Account.Authentication.OAuth
{
    // Maps provider names/strings to the OAuthProvider enum
    // and provides callback paths and support checks.

    public enum OAuthProvider
    {
        Unknown = 0,
        Google = 1,
        Facebook = 2
    }

    public static class OAuthProviderExtensions
    {
        // Parses a provider string (from URL or request) into the enum.
        public static OAuthProvider FromString(string provider)
        {
            return provider.ToLowerInvariant() switch
            {
                "google" or "google-oauth2" => OAuthProvider.Google,
                "facebook" or "facebook-login" => OAuthProvider.Facebook,
                _ => OAuthProvider.Unknown
            };
        }

        // Callback route for each provider — where they redirect after auth.
        public static string CallbackPath(this OAuthProvider provider)
        {
            return provider switch
            {
                OAuthProvider.Google => "/api/auth/callback/google",
                OAuthProvider.Facebook => "/api/auth/callback/facebook",
                _ => string.Empty
            };
        }

        // Whether this provider is currently configured and usable.
        public static bool IsSupported(this OAuthProvider provider)
        {
            return provider is OAuthProvider.Google or OAuthProvider.Facebook;
        }
    }

    // Holds all provider configs. Loaded from appsettings via OAuthConfigLoader.

    public sealed class OauthConfig
    {
        public const string SectionName = "OAuth";

        public GoogleProvider Google { get; init; } = default!;
        public FacebookProvider Facebook { get; init; } = default!;
    }
}
