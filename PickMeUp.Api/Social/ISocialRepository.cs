namespace PickMeUp.Api.Social;

public interface ISocialRepository
{
    Task EnsureUserAsync(string userId);
    Task<bool> AreFriendsAsync(string userA, string userB);
    Task AddFriendAsync(string userA, string userB);
    Task RemoveFriendAsync(string userA, string userB);
    Task<IReadOnlyList<string>> GetFriendsAsync(string userId);

    Task<bool> IsBlockedAsync(string blockerId, string targetId);
    Task<bool> IsBlockedEitherWayAsync(string userA, string userB);
    Task AddBlockAsync(string blockerId, string targetId);
    Task RemoveBlockAsync(string blockerId, string targetId);
    Task<IReadOnlyList<string>> GetBlocksAsync(string userId);

    Task<FriendRequestDocument?> FindPendingFriendRequestAsync(string senderId, string receiverId);
    Task<bool> HasPendingFriendRequestEitherWayAsync(string userA, string userB);
    Task<IReadOnlyList<FriendRequestDocument>> ListPendingFriendRequestsAsync(string userId);
    Task AddFriendRequestAsync(FriendRequestDocument request);
    Task UpdateFriendRequestStatusAsync(Guid id, FriendRequestStatus status, DateTime respondedAt);
    Task VoidPendingFriendRequestsBetweenAsync(string userA, string userB);

    Task<PartyInviteDocument?> FindPendingPartyInviteAsync(string senderId, string receiverId);
    Task<bool> HasPendingPartyInviteEitherWayAsync(string userA, string userB);
    Task<IReadOnlyList<PartyInviteDocument>> ListPendingPartyInvitesAsync(string userId);
    Task AddPartyInviteAsync(PartyInviteDocument invite);
    Task UpdatePartyInviteStatusAsync(Guid id, PartyInviteStatus status, DateTime respondedAt);
    Task VoidPendingPartyInvitesBetweenAsync(string userA, string userB);

    Task ExpireDueAsync(DateTime utcNow);
}
