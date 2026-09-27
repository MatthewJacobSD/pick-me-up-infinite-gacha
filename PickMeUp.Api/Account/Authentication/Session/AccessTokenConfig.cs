namespace PickMeUp.Api.Account.Authentication.Session
{
    /// <summary>
    /// Access token config — short-lived, measured in minutes.
    /// </summary>
    public sealed class AccessTokenConfig : TokenConfig
    {
        private AccessTokenConfig(string key, int expireInMinutes)
            : base(key, expireInMinutes, expireInDays: 0) { }

        public static AccessTokenConfig Create(string key, int expireInMinutes)
        {
            return new AccessTokenConfig(key, expireInMinutes);
        }
    }
}
