namespace PickMeUp.Api.Account.Authentication.OAuth.Provider
{
    // Facebook OAuth provider config value object.
    // Created by OAuthConfigLoader from appsettings, stored in OauthConfig.

    public sealed class FacebookProvider
    {
        public string AppId { get; private init; } = string.Empty;
        public string AppSecret { get; private init; } = string.Empty;

        private FacebookProvider(string appId, string appSecret)
        {
            AppId = appId;
            AppSecret = appSecret;
        }

        // Validates and creates the provider. Throws if credentials look invalid.
        public static FacebookProvider Create(string appId, string appSecret)
        {
            if (appId is null)
                throw new ArgumentException("Facebook App Id cannot be null");

            appId = appId.Trim();

            if (string.IsNullOrWhiteSpace(appId))
                throw new ArgumentException("Facebook App Id cannot be empty");

            if (appSecret is null)
                throw new ArgumentException("Facebook App Secret cannot be null");

            appSecret = appSecret.Trim();

            if (string.IsNullOrWhiteSpace(appSecret))
                throw new ArgumentException("Facebook App Secret cannot be empty");

            if (appSecret.Length < 10)
                throw new ArgumentException("Facebook App Secret looks invalid");

            return new FacebookProvider(appId, appSecret);
        }

        public override string ToString() => $"FacebookProvider(AppId={AppId})";
    }
}
