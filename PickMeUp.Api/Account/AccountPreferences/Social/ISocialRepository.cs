namespace PickMeUp.Api.Account.AccountPreferences.Social
{
    // ── Social Repository Interface ────────────────────
    // Persistence for friends, blocks, friend requests, and party invites.

    public interface ISocialRepository
    {
        // ── Friends ────────────────────────────────────
        Task<bool> AreFriendsAsync(string userA, string userB);
        Task AddFriendAsync(string userA, string userB);
        Task RemoveFriendAsync(string userA, string userB);
        Task<IReadOnlyList<string>> GetFriendsAsync(string userId);

        // ── Blocks ─────────────────────────────────────
        Task<bool> IsBlockedAsync(string blockerId, string targetId);
        Task AddBlockAsync(string blockerId, string targetId);
        Task RemoveBlockAsync(string blockerId, string targetId);
        Task<IReadOnlyList<string>> GetBlocksAsync(string userId);

        // ── Friend Requests ────────────────────────────
        Task<bool> HasPendingRequestAsync(string senderId, string receiverId);
        Task<FriendRequest?> GetFriendRequestAsync(string senderId, string receiverId);
        Task AddFriendRequestAsync(FriendRequest request);
        Task UpdateFriendRequestStatusAsync(string senderId, string receiverId, FriendRequestStatus status);

        // ── Party Invites ──────────────────────────────
        Task AddPartyInviteAsync(PartyInvite invite);
        Task<PartyInvite?> GetPartyInviteAsync(string senderId, string receiverId);
        Task UpdatePartyInviteStatusAsync(string senderId, string receiverId, PartyInviteStatus status);
    }
}
