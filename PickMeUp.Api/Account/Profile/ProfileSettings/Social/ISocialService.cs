namespace PickMeUp.Api.Account.Profile.ProfileSettings.Social
{
    // Service interface for social operations.
    // Orchestrates commands through the repository and lifecycle engine.

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
