namespace PickMeUp.Api.Account.AccountPreferences.SocialPreferences;

public sealed record SocialPatchDto
{
    public SocialVisibility? FriendRequests { get; init; }
    public SocialVisibility? Messages { get; init; }
    public SocialVisibility? PartyInvites { get; init; }
    public SocialVisibility? OnlineStatus { get; init; }
    public int Version { get; init; }

    public SocialSettings ApplyTo(SocialSettings current) => current with
    {
        FriendRequests = FriendRequests ?? current.FriendRequests,
        Messages = Messages ?? current.Messages,
        PartyInvites = PartyInvites ?? current.PartyInvites,
        OnlineStatus = OnlineStatus ?? current.OnlineStatus
    };
}
