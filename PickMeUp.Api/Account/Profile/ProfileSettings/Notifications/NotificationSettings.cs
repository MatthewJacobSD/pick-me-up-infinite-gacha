namespace PickMeUp.Api.Account.Profile.ProfileSettings.Notifications
{
    public sealed class NotificationSettings
    {
        public bool EventNotifications { get; init; } = true;
        public bool FriendRequestNotifications { get; init; } = true;
        public bool PartyInviteNotifications { get; init; } = true;
        public bool SystemAnnouncements { get; init; } = true;
        public bool RewardNotifications { get; init; } = true;
    }
}
