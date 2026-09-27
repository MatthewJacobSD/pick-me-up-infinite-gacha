using PickMeUp.Api.Account.Authentication.OAuth.Provider;

namespace PickMeUp.Api.Account.Authentication.OAuth
{
    /// <summary>
    /// Reads provider credentials from IConfiguration and builds an <see cref="OauthConfig"/>.
    /// </summary>
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
                _config["OAuth:Facebook:ClientId"]!,
                _config["OAuth:Facebook:ClientSecret"]!
            );

            return new OauthConfig
            {
                Google = google,
                Facebook = facebook
            };
        }
    }
}
