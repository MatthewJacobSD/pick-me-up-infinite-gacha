namespace PickMeUp.Api.Account.Authentication.Session
{
    // Base class for token configuration (access and refresh).

    public abstract class TokenConfig
    {
        public string Key { get; }
        public int ExpireInMinutes { get; }
        public int ExpireInDays { get; }

        protected TokenConfig(string key, int expireInMinutes, int expireInDays)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Token key cannot be empty");

            if (expireInMinutes < 0)
                throw new ArgumentException("ExpireInMinutes cannot be negative");

            if (expireInDays < 0)
                throw new ArgumentException("ExpireInDays cannot be negative");

            Key = key;
            ExpireInMinutes = expireInMinutes;
            ExpireInDays = expireInDays;
        }
    }
}
