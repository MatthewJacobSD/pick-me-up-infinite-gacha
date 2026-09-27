namespace PickMeUp.Api.Account.AccountPreferences.SocialPreferences;

/// <summary>
/// Partial-update DTO for social visibility preferences (PATCH). Null fields are left unchanged.
/// </summary>
public sealed record SocialPatchDto
{
    public SocialVisibility? FriendRequests { get; init; }
    public SocialVisibility? Messages { get; init; }
    public SocialVisibility? PartyInvites { get; init; }
    public SocialVisibility? OnlineStatus { get; init; }
    public int Version { get; init; }

    /// <summary>Merges non-null patch values onto <paramref name="current"/>, returning a new record.</summary>
    public SocialSettings ApplyTo(SocialSettings current) => current with
    {
        FriendRequests = FriendRequests ?? current.FriendRequests,
        Messages = Messages ?? current.Messages,
        PartyInvites = PartyInvites ?? current.PartyInvites,
        OnlineStatus = OnlineStatus ?? current.OnlineStatus
    };
}
