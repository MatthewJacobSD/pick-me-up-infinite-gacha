namespace PickMeUp.Api.Account.Profile.ProfileSettings.Notifications
{
    public sealed class NotificationSettingsDto
    {
        public bool EventNotifications { get; init; }
        public bool FriendRequestNotifications { get; init; }
        public bool PartyInviteNotifications { get; init; }
        public bool SystemAnnouncements { get; init; }
        public bool RewardNotifications { get; init; }
    }
}
