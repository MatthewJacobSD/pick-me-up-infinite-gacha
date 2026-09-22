namespace PickMeUp.Api.Social
{
    // ── Social Service ─────────────────────────────────
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

        // ── Friends ────────────────────────────────────

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

        // ── Blocks ─────────────────────────────────────

        public async Task<IReadOnlyList<string>> GetBlocksAsync(string userId)
        {
            return await _repo.GetBlocksAsync(userId);
        }

        // 1. Remove existing friendship if present.
        // 2. Add block.
        public async Task BlockUserAsync(string userId, BlockCommand command)
        {
            if (await _repo.AreFriendsAsync(userId, command.TargetUserId))
                await _repo.RemoveFriendAsync(userId, command.TargetUserId);

            await _repo.AddBlockAsync(userId, command.TargetUserId);
        }

        public async Task UnblockUserAsync(string userId, string targetUserId)
        {
            await _repo.RemoveBlockAsync(userId, targetUserId);
        }

        // ── Party ──────────────────────────────────────

        // 1. Verify target has not blocked the sender.
        // 2. Create and persist pending invite.
        public async Task HandlePartyInviteAsync(string userId, PartyCommand command)
        {
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
