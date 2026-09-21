namespace PickMeUp.Api.Account.Authentication.Session
{
    // Session config controlling lifetime and optional bindings.

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
