namespace PickMeUp.Api.Account.AccountPreferences.Notifications
{
    public sealed record NotificationSettings
    {
        public static NotificationSettings Default { get; } = new();

        public bool EventNotifications { get; init; } = true;
        public bool FriendRequestNotifications { get; init; } = true;
        public bool PartyInviteNotifications { get; init; } = true;
        public bool SystemAnnouncements { get; init; } = true;
        public bool RewardNotifications { get; init; } = true;
    }
}
