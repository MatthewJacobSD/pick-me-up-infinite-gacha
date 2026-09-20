using PickMeUp.Api.Account.Authentication.OAuth.Provider;

namespace PickMeUp.Api.Account.Authentication.OAuth
{
    // Reads provider credentials from IConfiguration and builds an OauthConfig.
    // Called once at startup to populate OAuthProviderRegistry.

    public sealed class OAuthConfigLoader(IConfiguration config)
    {
        private readonly IConfiguration _config = config;

        public OauthConfig Load()
        {
            var google = GoogleProvider.Create(
                _config["OAuth:Google:ClientId"]!,
                _config["OAuth:Google:ClientSecret"]!
            );

            var facebook = FacebookProvider.Create(
                _config["OAuth:Facebook:AppId"]!,
                _config["OAuth:Facebook:AppSecret"]!
            );

            return new OauthConfig
            {
                Google = google,
                Facebook = facebook
            };
        }
    }
}
