namespace PickMeUp.Api.Social;

/**--------[ISocialService]--------**/

/// <summary>
/// Application-layer facade for all social operations. Enforces policy and delegates persistence.
/// </summary>
public interface ISocialService
{
    /**--------[Friends]--------**/

    Task<IReadOnlyList<string>> GetFriendsAsync(string userId);
    Task SendFriendRequestAsync(string actorId, string targetId);
    Task AcceptFriendRequestAsync(string actorId, string senderId);
    Task DeclineFriendRequestAsync(string actorId, string senderId);
    Task CancelFriendRequestAsync(string actorId, string receiverId);
    Task<IReadOnlyList<FriendRequestDocument>> ListPendingFriendRequestsAsync(string userId);
    Task RemoveFriendAsync(string userId, string targetUserId);

    /**--------[Blocks]--------**/

    Task<IReadOnlyList<string>> GetBlocksAsync(string userId);
    Task BlockUserAsync(string userId, string targetUserId);
    Task UnblockUserAsync(string userId, string targetUserId);

    /**--------[Party Invites]--------**/

    Task SendPartyInviteAsync(string actorId, string targetId);
    Task AcceptPartyInviteAsync(string actorId, string senderId);
    Task DeclinePartyInviteAsync(string actorId, string senderId);
    Task CancelPartyInviteAsync(string actorId, string receiverId);
    Task<IReadOnlyList<PartyInviteDocument>> ListPendingPartyInvitesAsync(string userId);
}
