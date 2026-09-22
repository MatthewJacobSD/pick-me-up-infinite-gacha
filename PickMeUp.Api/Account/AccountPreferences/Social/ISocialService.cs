namespace PickMeUp.Api.Account.AccountPreferences.Social
{
    // ── Social Service Interface ───────────────────────
    // Orchestrates social commands through the repository and lifecycle engine.

    public interface ISocialService
    {
        Task<IReadOnlyList<string>> GetFriendsAsync(string userId);
        Task SendFriendRequestAsync(string userId, FriendCommand command);
        Task RemoveFriendAsync(string userId, string targetUserId);

        Task<IReadOnlyList<string>> GetBlocksAsync(string userId);
        Task BlockUserAsync(string userId, BlockCommand command);
        Task UnblockUserAsync(string userId, string targetUserId);

        Task HandlePartyInviteAsync(string userId, PartyCommand command);
    }
}
