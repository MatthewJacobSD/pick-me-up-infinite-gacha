namespace PickMeUp.Api.Account.Authentication.Session
{
    /// <summary>
    /// Refresh token config — long-lived, measured in days.
    /// </summary>
    public sealed class RefreshTokenConfig : TokenConfig
    {
        private RefreshTokenConfig(string key, int expireInDays)
            : base(key, expireInMinutes: 0, expireInDays) { }

        public static RefreshTokenConfig Create(string key, int expireInDays)
        {
            return new RefreshTokenConfig(key, expireInDays);
        }
    }
}
