namespace PickMeUp.Api.Account.AccountPreferences.Notifications
{
    public sealed class NotificationSettingsDto
    {
        public bool EventNotifications { get; init; }
        public bool FriendRequestNotifications { get; init; }
        public bool PartyInviteNotifications { get; init; }
        public bool SystemAnnouncements { get; init; }
        public bool RewardNotifications { get; init; }

        public int Version { get; init; }

        public NotificationSettings ToSettings() => new()
        {
            EventNotifications = EventNotifications,
            FriendRequestNotifications = FriendRequestNotifications,
            PartyInviteNotifications = PartyInviteNotifications,
            SystemAnnouncements = SystemAnnouncements,
            RewardNotifications = RewardNotifications
        };
    }
}
