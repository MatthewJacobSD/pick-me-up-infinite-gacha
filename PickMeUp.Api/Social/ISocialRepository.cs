namespace PickMeUp.Api.Social;

/**--------[ISocialRepository]--------**/

/// <summary>
/// Data access for all social state: friendships, blocks, friend requests, and party invites.
/// </summary>
public interface ISocialRepository
{
    /**--------[Users]--------**/

    Task EnsureUserAsync(string userId);
    Task<bool> AreFriendsAsync(string userA, string userB);
    Task AddFriendAsync(string userA, string userB);
    Task RemoveFriendAsync(string userA, string userB);
    Task<IReadOnlyList<string>> GetFriendsAsync(string userId);

    /**--------[Blocks]--------**/

    Task<bool> IsBlockedAsync(string blockerId, string targetId);
    Task<bool> IsBlockedEitherWayAsync(string userA, string userB);
    Task AddBlockAsync(string blockerId, string targetId);
    Task RemoveBlockAsync(string blockerId, string targetId);
    Task<IReadOnlyList<string>> GetBlocksAsync(string userId);

    /**--------[Friend Requests]--------**/

    Task<FriendRequestDocument?> FindPendingFriendRequestAsync(string senderId, string receiverId);
    Task<bool> HasPendingFriendRequestEitherWayAsync(string userA, string userB);
    Task<IReadOnlyList<FriendRequestDocument>> ListPendingFriendRequestsAsync(string userId);
    Task AddFriendRequestAsync(FriendRequestDocument request);
    Task UpdateFriendRequestStatusAsync(Guid id, FriendRequestStatus status, DateTime respondedAt);
    Task VoidPendingFriendRequestsBetweenAsync(string userA, string userB);

    /**--------[Party Invites]--------**/

    Task<PartyInviteDocument?> FindPendingPartyInviteAsync(string senderId, string receiverId);
    Task<bool> HasPendingPartyInviteEitherWayAsync(string userA, string userB);
    Task<IReadOnlyList<PartyInviteDocument>> ListPendingPartyInvitesAsync(string userId);
    Task AddPartyInviteAsync(PartyInviteDocument invite);
    Task UpdatePartyInviteStatusAsync(Guid id, PartyInviteStatus status, DateTime respondedAt);
    Task VoidPendingPartyInvitesBetweenAsync(string userA, string userB);

    /**--------[Expiry]--------**/

    Task ExpireDueAsync(DateTime utcNow);
}
