namespace PickMeUp.Api.Social;

public interface IFriendRequestLifecycleEngine
{
    Task SendAsync(string senderId, string receiverId);
    Task AcceptAsync(string receiverId, string senderId);
    Task DeclineAsync(string receiverId, string senderId);
    Task CancelAsync(string senderId, string receiverId);
    Task ExpireAsync(string senderId, string receiverId);

    Task SendPartyAsync(string senderId, string receiverId);
    Task AcceptPartyAsync(string receiverId, string senderId);
    Task DeclinePartyAsync(string receiverId, string senderId);
    Task CancelPartyAsync(string senderId, string receiverId);
}

public sealed class FriendRequestLifecycleEngine(ISocialRepository repository) : IFriendRequestLifecycleEngine
{
    private readonly ISocialRepository _repository = repository;

    public async Task SendAsync(string senderId, string receiverId)
    {
        if (await _repository.AreFriendsAsync(senderId, receiverId))
            throw new SocialConflictException("social.already_friends", "These players are already friends.");

        if (await _repository.HasPendingFriendRequestEitherWayAsync(senderId, receiverId))
            throw new SocialConflictException("social.request_exists", "A pending friend request already exists.");

        var now = DateTime.UtcNow;
        await _repository.AddFriendRequestAsync(new FriendRequestDocument
        {
            Id = Guid.NewGuid(),
            SenderId = senderId,
            ReceiverId = receiverId,
            Status = FriendRequestStatus.Pending,
            CreatedAt = now,
            ExpiresAt = now.Add(SocialIndexDefinitions.RequestLifetime)
        });
    }

    public async Task AcceptAsync(string receiverId, string senderId)
    {
        var request = await _repository.FindPendingFriendRequestAsync(senderId, receiverId);
        if (request is null)
            throw new SocialConflictException("social.no_pending", "No pending friend request to accept.");

        await _repository.UpdateFriendRequestStatusAsync(request.Id, FriendRequestStatus.Accepted, DateTime.UtcNow);
        await _repository.AddFriendAsync(senderId, receiverId);
    }

    public async Task DeclineAsync(string receiverId, string senderId)
    {
        var request = await _repository.FindPendingFriendRequestAsync(senderId, receiverId);
        if (request is null)
            throw new SocialConflictException("social.no_pending", "No pending friend request to decline.");

        await _repository.UpdateFriendRequestStatusAsync(request.Id, FriendRequestStatus.Declined, DateTime.UtcNow);
    }

    public async Task CancelAsync(string senderId, string receiverId)
    {
        var request = await _repository.FindPendingFriendRequestAsync(senderId, receiverId);
        if (request is null)
            throw new SocialConflictException("social.no_pending", "No pending friend request to cancel.");

        await _repository.UpdateFriendRequestStatusAsync(request.Id, FriendRequestStatus.Cancelled, DateTime.UtcNow);
    }

    public async Task ExpireAsync(string senderId, string receiverId)
    {
        var request = await _repository.FindPendingFriendRequestAsync(senderId, receiverId);
        if (request is null)
            return;

        await _repository.UpdateFriendRequestStatusAsync(request.Id, FriendRequestStatus.Expired, DateTime.UtcNow);
    }

    public async Task SendPartyAsync(string senderId, string receiverId)
    {
        if (await _repository.HasPendingPartyInviteEitherWayAsync(senderId, receiverId))
            throw new SocialConflictException("social.invite_exists", "A pending party invite already exists.");

        var now = DateTime.UtcNow;
        await _repository.AddPartyInviteAsync(new PartyInviteDocument
        {
            Id = Guid.NewGuid(),
            SenderId = senderId,
            ReceiverId = receiverId,
            Status = PartyInviteStatus.Pending,
            CreatedAt = now,
            ExpiresAt = now.Add(SocialIndexDefinitions.RequestLifetime)
        });
    }

    public async Task AcceptPartyAsync(string receiverId, string senderId)
    {
        var invite = await _repository.FindPendingPartyInviteAsync(senderId, receiverId);
        if (invite is null)
            throw new SocialConflictException("social.no_pending", "No pending party invite to accept.");

        await _repository.UpdatePartyInviteStatusAsync(invite.Id, PartyInviteStatus.Accepted, DateTime.UtcNow);
    }

    public async Task DeclinePartyAsync(string receiverId, string senderId)
    {
        var invite = await _repository.FindPendingPartyInviteAsync(senderId, receiverId);
        if (invite is null)
            throw new SocialConflictException("social.no_pending", "No pending party invite to decline.");

        await _repository.UpdatePartyInviteStatusAsync(invite.Id, PartyInviteStatus.Declined, DateTime.UtcNow);
    }

    public async Task CancelPartyAsync(string senderId, string receiverId)
    {
        var invite = await _repository.FindPendingPartyInviteAsync(senderId, receiverId);
        if (invite is null)
            throw new SocialConflictException("social.no_pending", "No pending party invite to cancel.");

        await _repository.UpdatePartyInviteStatusAsync(invite.Id, PartyInviteStatus.Cancelled, DateTime.UtcNow);
    }
}
