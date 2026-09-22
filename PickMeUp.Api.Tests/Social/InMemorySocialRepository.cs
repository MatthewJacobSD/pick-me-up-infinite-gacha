using PickMeUp.Api.Social;

namespace PickMeUp.Api.Tests.Social;

public sealed class InMemorySocialRepository : ISocialRepository
{
    private readonly Dictionary<string, SocialDocument> _users = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<FriendRequestDocument> _requests = [];
    private readonly List<PartyInviteDocument> _invites = [];

    public Task EnsureUserAsync(string userId)
    {
        if (!_users.ContainsKey(userId))
        {
            _users[userId] = new SocialDocument { UserId = userId };
        }

        return Task.CompletedTask;
    }

    public Task<bool> AreFriendsAsync(string userA, string userB)
        => Task.FromResult(_users.TryGetValue(userA, out var doc) && doc.Friends.Contains(userB));

    public async Task AddFriendAsync(string userA, string userB)
    {
        await EnsureUserAsync(userA);
        await EnsureUserAsync(userB);
        AddOnce(_users[userA].Friends, userB);
        AddOnce(_users[userB].Friends, userA);
    }

    public async Task RemoveFriendAsync(string userA, string userB)
    {
        await EnsureUserAsync(userA);
        await EnsureUserAsync(userB);
        _users[userA].Friends.Remove(userB);
        _users[userB].Friends.Remove(userA);
    }

    public Task<IReadOnlyList<string>> GetFriendsAsync(string userId)
        => Task.FromResult<IReadOnlyList<string>>(_users.TryGetValue(userId, out var doc) ? doc.Friends.ToArray() : []);

    public Task<bool> IsBlockedAsync(string blockerId, string targetId)
        => Task.FromResult(_users.TryGetValue(blockerId, out var doc) && doc.Blocks.Contains(targetId));

    public async Task<bool> IsBlockedEitherWayAsync(string userA, string userB)
        => await IsBlockedAsync(userA, userB) || await IsBlockedAsync(userB, userA);

    public async Task AddBlockAsync(string blockerId, string targetId)
    {
        await EnsureUserAsync(blockerId);
        AddOnce(_users[blockerId].Blocks, targetId);
    }

    public Task RemoveBlockAsync(string blockerId, string targetId)
    {
        if (_users.TryGetValue(blockerId, out var doc))
            doc.Blocks.Remove(targetId);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<string>> GetBlocksAsync(string userId)
        => Task.FromResult<IReadOnlyList<string>>(_users.TryGetValue(userId, out var doc) ? doc.Blocks.ToArray() : []);

    public async Task<FriendRequestDocument?> FindPendingFriendRequestAsync(string senderId, string receiverId)
    {
        await ExpireDueAsync(DateTime.UtcNow);
        return _requests.FirstOrDefault(x =>
            x.SenderId == senderId &&
            x.ReceiverId == receiverId &&
            x.Status == FriendRequestStatus.Pending);
    }

    public async Task<bool> HasPendingFriendRequestEitherWayAsync(string userA, string userB)
    {
        await ExpireDueAsync(DateTime.UtcNow);
        return _requests.Any(x => x.Status == FriendRequestStatus.Pending && Pair(x.SenderId, x.ReceiverId, userA, userB));
    }

    public async Task<IReadOnlyList<FriendRequestDocument>> ListPendingFriendRequestsAsync(string userId)
    {
        await ExpireDueAsync(DateTime.UtcNow);
        return _requests.Where(x =>
            x.Status == FriendRequestStatus.Pending &&
            (x.SenderId == userId || x.ReceiverId == userId)).ToArray();
    }

    public Task AddFriendRequestAsync(FriendRequestDocument request)
    {
        if (request.Status == FriendRequestStatus.Pending &&
            _requests.Any(x => x.Status == FriendRequestStatus.Pending && x.SenderId == request.SenderId && x.ReceiverId == request.ReceiverId))
        {
            throw new SocialConflictException("social.request_exists", "A pending friend request already exists.");
        }

        _requests.Add(request);
        return Task.CompletedTask;
    }

    public Task UpdateFriendRequestStatusAsync(Guid id, FriendRequestStatus status, DateTime respondedAt)
    {
        var request = _requests.FirstOrDefault(x => x.Id == id && x.Status == FriendRequestStatus.Pending);
        if (request is null)
            return Task.CompletedTask;
        request.Status = status;
        request.RespondedAt = respondedAt;
        return Task.CompletedTask;
    }

    public Task VoidPendingFriendRequestsBetweenAsync(string userA, string userB)
    {
        foreach (var request in _requests.Where(x => x.Status == FriendRequestStatus.Pending && Pair(x.SenderId, x.ReceiverId, userA, userB)))
        {
            request.Status = FriendRequestStatus.Cancelled;
            request.RespondedAt = DateTime.UtcNow;
        }

        return Task.CompletedTask;
    }

    public async Task<PartyInviteDocument?> FindPendingPartyInviteAsync(string senderId, string receiverId)
    {
        await ExpireDueAsync(DateTime.UtcNow);
        return _invites.FirstOrDefault(x =>
            x.SenderId == senderId &&
            x.ReceiverId == receiverId &&
            x.Status == PartyInviteStatus.Pending);
    }

    public async Task<bool> HasPendingPartyInviteEitherWayAsync(string userA, string userB)
    {
        await ExpireDueAsync(DateTime.UtcNow);
        return _invites.Any(x => x.Status == PartyInviteStatus.Pending && Pair(x.SenderId, x.ReceiverId, userA, userB));
    }

    public async Task<IReadOnlyList<PartyInviteDocument>> ListPendingPartyInvitesAsync(string userId)
    {
        await ExpireDueAsync(DateTime.UtcNow);
        return _invites.Where(x =>
            x.Status == PartyInviteStatus.Pending &&
            (x.SenderId == userId || x.ReceiverId == userId)).ToArray();
    }

    public Task AddPartyInviteAsync(PartyInviteDocument invite)
    {
        if (invite.Status == PartyInviteStatus.Pending &&
            _invites.Any(x => x.Status == PartyInviteStatus.Pending && x.SenderId == invite.SenderId && x.ReceiverId == invite.ReceiverId))
        {
            throw new SocialConflictException("social.invite_exists", "A pending party invite already exists.");
        }

        _invites.Add(invite);
        return Task.CompletedTask;
    }

    public Task UpdatePartyInviteStatusAsync(Guid id, PartyInviteStatus status, DateTime respondedAt)
    {
        var invite = _invites.FirstOrDefault(x => x.Id == id && x.Status == PartyInviteStatus.Pending);
        if (invite is null)
            return Task.CompletedTask;
        invite.Status = status;
        invite.RespondedAt = respondedAt;
        return Task.CompletedTask;
    }

    public Task VoidPendingPartyInvitesBetweenAsync(string userA, string userB)
    {
        foreach (var invite in _invites.Where(x => x.Status == PartyInviteStatus.Pending && Pair(x.SenderId, x.ReceiverId, userA, userB)))
        {
            invite.Status = PartyInviteStatus.Cancelled;
            invite.RespondedAt = DateTime.UtcNow;
        }

        return Task.CompletedTask;
    }

    public Task ExpireDueAsync(DateTime utcNow)
    {
        foreach (var request in _requests.Where(x => x.Status == FriendRequestStatus.Pending && x.ExpiresAt < utcNow))
        {
            request.Status = FriendRequestStatus.Expired;
            request.RespondedAt = utcNow;
        }

        foreach (var invite in _invites.Where(x => x.Status == PartyInviteStatus.Pending && x.ExpiresAt < utcNow))
        {
            invite.Status = PartyInviteStatus.Expired;
            invite.RespondedAt = utcNow;
        }

        return Task.CompletedTask;
    }

    private static void AddOnce(List<string> list, string id)
    {
        if (!list.Contains(id))
            list.Add(id);
    }

    private static bool Pair(string sender, string receiver, string userA, string userB)
        => (sender == userA && receiver == userB) || (sender == userB && receiver == userA);
}
