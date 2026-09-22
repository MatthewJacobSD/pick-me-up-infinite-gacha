namespace PickMeUp.Api.Social;

public sealed class SocialService(
    ISocialRepository repository,
    IFriendRequestLifecycleEngine lifecycle,
    SocialPolicy policy) : ISocialService
{
    private readonly ISocialRepository _repository = repository;
    private readonly IFriendRequestLifecycleEngine _lifecycle = lifecycle;
    private readonly SocialPolicy _policy = policy;

    public Task<IReadOnlyList<string>> GetFriendsAsync(string userId)
        => _repository.GetFriendsAsync(userId);

    public async Task SendFriendRequestAsync(string actorId, string targetId)
    {
        RequireOtherPlayer(actorId, targetId);
        await RequireAllowed(await _policy.CanSendFriendRequest(actorId, targetId));
        await _lifecycle.SendAsync(actorId, targetId);
    }

    public async Task AcceptFriendRequestAsync(string actorId, string senderId)
    {
        RequireOtherPlayer(actorId, senderId);
        await RequireNotBlocked(actorId, senderId);
        await _lifecycle.AcceptAsync(actorId, senderId);
    }

    public async Task DeclineFriendRequestAsync(string actorId, string senderId)
    {
        RequireOtherPlayer(actorId, senderId);
        await _lifecycle.DeclineAsync(actorId, senderId);
    }

    public async Task CancelFriendRequestAsync(string actorId, string receiverId)
    {
        RequireOtherPlayer(actorId, receiverId);
        await _lifecycle.CancelAsync(actorId, receiverId);
    }

    public Task<IReadOnlyList<FriendRequestDocument>> ListPendingFriendRequestsAsync(string userId)
        => _repository.ListPendingFriendRequestsAsync(userId);

    public Task RemoveFriendAsync(string userId, string targetUserId)
    {
        RequireOtherPlayer(userId, targetUserId);
        return _repository.RemoveFriendAsync(userId, targetUserId);
    }

    public Task<IReadOnlyList<string>> GetBlocksAsync(string userId)
        => _repository.GetBlocksAsync(userId);

    public async Task BlockUserAsync(string userId, string targetUserId)
    {
        RequireOtherPlayer(userId, targetUserId);
        await _repository.RemoveFriendAsync(userId, targetUserId);
        await _repository.VoidPendingFriendRequestsBetweenAsync(userId, targetUserId);
        await _repository.VoidPendingPartyInvitesBetweenAsync(userId, targetUserId);
        await _repository.AddBlockAsync(userId, targetUserId);
    }

    public Task UnblockUserAsync(string userId, string targetUserId)
    {
        RequireOtherPlayer(userId, targetUserId);
        return _repository.RemoveBlockAsync(userId, targetUserId);
    }

    public async Task SendPartyInviteAsync(string actorId, string targetId)
    {
        RequireOtherPlayer(actorId, targetId);
        await RequireAllowed(await _policy.CanInviteToParty(actorId, targetId));
        await _lifecycle.SendPartyAsync(actorId, targetId);
    }

    public async Task AcceptPartyInviteAsync(string actorId, string senderId)
    {
        RequireOtherPlayer(actorId, senderId);
        await RequireNotBlocked(actorId, senderId);
        await _lifecycle.AcceptPartyAsync(actorId, senderId);
    }

    public async Task DeclinePartyInviteAsync(string actorId, string senderId)
    {
        RequireOtherPlayer(actorId, senderId);
        await _lifecycle.DeclinePartyAsync(actorId, senderId);
    }

    public async Task CancelPartyInviteAsync(string actorId, string receiverId)
    {
        RequireOtherPlayer(actorId, receiverId);
        await _lifecycle.CancelPartyAsync(actorId, receiverId);
    }

    public Task<IReadOnlyList<PartyInviteDocument>> ListPendingPartyInvitesAsync(string userId)
        => _repository.ListPendingPartyInvitesAsync(userId);

    private async Task RequireNotBlocked(string actorId, string otherId)
    {
        if (await _repository.IsBlockedEitherWayAsync(actorId, otherId))
        {
            throw new SocialPolicyDeniedException(
                "social.blocked",
                "A block between these players prevents this action.");
        }
    }

    private static void RequireOtherPlayer(string actorId, string otherId)
    {
        if (string.IsNullOrWhiteSpace(otherId) || !Guid.TryParse(otherId, out _))
            throw new SocialValidationException("Target account id is not a valid account id.");

        if (string.Equals(actorId, otherId, StringComparison.OrdinalIgnoreCase))
            throw new SocialValidationException("A player cannot target their own account.");
    }

    private static Task RequireAllowed(SocialDecision decision)
    {
        if (decision.Allowed)
            return Task.CompletedTask;

        if (decision.Denial == SocialDenialKind.Blocked)
        {
            throw new SocialPolicyDeniedException(
                "social.blocked",
                "A block between these players prevents this action.");
        }

        throw new SocialPolicyDeniedException(
            "social.visibility_denied",
            "The target player's visibility settings deny this action.");
    }
}
