namespace PickMeUp.Api.Account.Authentication.Session
{
    // Session config — controls session lifetime and optional bindings.
    // Loaded from appsettings via Jwt section.

    public sealed class SessionConfig
    {
        public int ExpireInHours { get; }
        public bool EnableDeviceBinding { get; }
        public bool EnableIpBinding { get; }

        private SessionConfig(int expireInHours, bool enableDeviceBinding, bool enableIpBinding)
        {
            if (expireInHours <= 0)
                throw new ArgumentException("Session expiration must be positive");

            ExpireInHours = expireInHours;
            EnableDeviceBinding = enableDeviceBinding;
            EnableIpBinding = enableIpBinding;
        }

        public static SessionConfig Create(
            int expireInHours,
            bool enableDeviceBinding = false,
            bool enableIpBinding = false)
        {
            return new SessionConfig(expireInHours, enableDeviceBinding, enableIpBinding);
        }
    }
}
