namespace PickMeUp.Api.Account.AccountPreferences.Notifications
{
    /// <summary>
    /// Full-replace DTO for notification preferences (PUT). All toggle flags are required.
    /// </summary>
    public sealed class NotificationDto
    {
        public bool EventNotifications { get; init; }
        public bool FriendRequestNotifications { get; init; }
        public bool PartyInviteNotifications { get; init; }
        public bool SystemAnnouncements { get; init; }
        public bool RewardNotifications { get; init; }

        public int Version { get; init; }

        /// <summary>Maps this DTO to the domain <see cref="NotificationSettings"/> record.</summary>
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
