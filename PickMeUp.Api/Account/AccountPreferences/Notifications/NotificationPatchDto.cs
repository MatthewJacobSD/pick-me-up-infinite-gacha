namespace PickMeUp.Api.Account.AccountPreferences.Notifications;

/// <summary>
/// Partial-update DTO for notification preferences (PATCH). Null fields are left unchanged.
/// </summary>
public sealed record NotificationPatchDto
{
    public bool? EventNotifications { get; init; }
    public bool? FriendRequestNotifications { get; init; }
    public bool? PartyInviteNotifications { get; init; }
    public bool? SystemAnnouncements { get; init; }
    public bool? RewardNotifications { get; init; }
    public int Version { get; init; }

    /// <summary>Merges non-null patch values onto <paramref name="current"/>, returning a new record.</summary>
    public NotificationSettings ApplyTo(NotificationSettings current) => current with
    {
        EventNotifications = EventNotifications ?? current.EventNotifications,
        FriendRequestNotifications = FriendRequestNotifications ?? current.FriendRequestNotifications,
        PartyInviteNotifications = PartyInviteNotifications ?? current.PartyInviteNotifications,
        SystemAnnouncements = SystemAnnouncements ?? current.SystemAnnouncements,
        RewardNotifications = RewardNotifications ?? current.RewardNotifications
    };
}
