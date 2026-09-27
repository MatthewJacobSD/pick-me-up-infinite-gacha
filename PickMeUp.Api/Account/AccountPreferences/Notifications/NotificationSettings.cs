namespace PickMeUp.Api.Account.AccountPreferences.Notifications
{
    /// <summary>
    /// Domain record for notification toggle preferences. All categories default to enabled.
    /// </summary>
    public sealed record NotificationSettings
    {
        /// <summary>Shared default instance — all notifications enabled.</summary>
        public static NotificationSettings Default { get; } = new();

        public bool EventNotifications { get; init; } = true;
        public bool FriendRequestNotifications { get; init; } = true;
        public bool PartyInviteNotifications { get; init; } = true;
        public bool SystemAnnouncements { get; init; } = true;
        public bool RewardNotifications { get; init; } = true;
    }
}
