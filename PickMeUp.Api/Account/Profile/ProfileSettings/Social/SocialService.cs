using PickMeUp.Api.Account.Profile.ProfileSettings.Social.Social;

namespace PickMeUp.Api.Account.Profile.ProfileSettings.Social
{
    // Concrete implementation of ISocialService.
    // Orchestrates friend/block/party commands through the repository
    // and lifecycle engine.

    public sealed class SocialService : ISocialService
    {
        private readonly ISocialRepository _repo;
        private readonly IFriendRequestLifecycleEngine _lifecycle;

        public SocialService(ISocialRepository repo, IFriendRequestLifecycleEngine lifecycle)
        {
            _repo = repo;
            _lifecycle = lifecycle;
        }

        // ── Friends ───────────────────────────────────────────────

        public async Task<IReadOnlyList<string>> GetFriendsAsync(string userId)
        {
            return await _repo.GetFriendsAsync(userId);
        }

        public async Task SendFriendRequestAsync(string userId, FriendCommand command)
        {
            await _lifecycle.SendAsync(userId, command.TargetUserId);
        }

        public async Task RemoveFriendAsync(string userId, string targetUserId)
        {
            await _repo.RemoveFriendAsync(userId, targetUserId);
        }

        // ── Blocks ────────────────────────────────────────────────

        public async Task<IReadOnlyList<string>> GetBlocksAsync(string userId)
        {
            return await _repo.GetBlocksAsync(userId);
        }

        public async Task BlockUserAsync(string userId, BlockCommand command)
        {
            // Remove friendship if it exists, then block.
            if (await _repo.AreFriendsAsync(userId, command.TargetUserId))
                await _repo.RemoveFriendAsync(userId, command.TargetUserId);

            await _repo.AddBlockAsync(userId, command.TargetUserId);
        }

        public async Task UnblockUserAsync(string userId, string targetUserId)
        {
            await _repo.RemoveBlockAsync(userId, targetUserId);
        }

        // ── Party ─────────────────────────────────────────────────

        public async Task HandlePartyInviteAsync(string userId, PartyCommand command)
        {
            // Block enforcement
            if (await _repo.IsBlockedAsync(command.TargetUserId, userId))
                throw new InvalidOperationException("Cannot invite blocked user.");

            var invite = new PartyInvite
            {
                FromUserId = userId,
                ToUserId = command.TargetUserId,
                Status = PartyInviteStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddPartyInviteAsync(invite);
        }
    }
}
