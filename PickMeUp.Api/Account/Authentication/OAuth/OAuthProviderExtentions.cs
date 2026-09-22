namespace PickMeUp.Api.Account.Authentication.OAuth
{
    // Maps provider names to the OAuthProvider enum with callback paths.

    public enum OAuthProvider
    {
        Unknown = 0,
        Google = 1,
        Facebook = 2
    }

    public static class OAuthProviderExtensions
    {
        public static OAuthProvider FromString(string provider)
        {
            return provider.ToLowerInvariant() switch
            {
                "google" or "google-oauth2" => OAuthProvider.Google,
                "facebook" or "facebook-login" => OAuthProvider.Facebook,
                _ => OAuthProvider.Unknown
            };
        }

        public static string CallbackPath(this OAuthProvider provider)
        {
            return provider switch
            {
                OAuthProvider.Google => "/api/auth/callback/google",
                OAuthProvider.Facebook => "/api/auth/callback/facebook",
                _ => string.Empty
            };
        }

        public static bool IsSupported(this OAuthProvider provider)
        {
            return provider is OAuthProvider.Google or OAuthProvider.Facebook;
        }
    }

    // Holds all provider configs loaded from appsettings.

    public sealed class OauthConfig
    {
        public const string SectionName = "OAuth";

        public Provider.GoogleProvider Google { get; init; } = default!;
        public Provider.FacebookProvider Facebook { get; init; } = default!;
    }
}
