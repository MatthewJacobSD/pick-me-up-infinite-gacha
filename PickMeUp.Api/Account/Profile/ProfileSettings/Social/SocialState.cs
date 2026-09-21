namespace PickMeUp.Api.Account.Profile.ProfileSettings.Social
{
    // Domain models for the social system.
    //
    // SocialState — read-only snapshot of a player's social data.
    // FriendRequest — tracks pending/accepted/declined/cancelled/expired states.
    // PartyInvite — tracks party invitation lifecycle.
    //
    // These are social domain entities, NOT settings.

    public sealed class SocialState
    {
        public IReadOnlyList<string> Friends { get; init; } = [];
        public IReadOnlyList<string> Blocks { get; init; } = [];
        public IReadOnlyList<FriendRequest> FriendRequests { get; init; } = [];
        public IReadOnlyList<PartyInvite> PartyInvites { get; init; } = [];
    }

    // A friend request between two players.
    public sealed class FriendRequest
    {
        public string FromUserId { get; init; } = string.Empty;
        public string ToUserId { get; init; } = string.Empty;
        public FriendRequestStatus Status { get; init; } = FriendRequestStatus.Pending;
        public DateTime CreatedAt { get; init; }
    }

    public enum FriendRequestStatus
    {
        Pending = 0,
        Accepted = 1,
        Declined = 2,
        Cancelled = 3,
        Expired = 4
    }

    // A party invitation between two players.
    public sealed class PartyInvite
    {
        public string FromUserId { get; init; } = string.Empty;
        public string ToUserId { get; init; } = string.Empty;
        public PartyInviteStatus Status { get; init; } = PartyInviteStatus.Pending;
        public DateTime CreatedAt { get; init; }
    }

    public enum PartyInviteStatus
    {
        Pending = 0,
        Accepted = 1,
        Declined = 2,
        Cancelled = 3,
        Expired = 4
    }
}
