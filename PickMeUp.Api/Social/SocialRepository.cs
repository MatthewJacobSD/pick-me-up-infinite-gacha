using MongoDB.Driver;

namespace PickMeUp.Api.Social;

public sealed class SocialRepository(IMongoDatabase database) : ISocialRepository
{
    private readonly IMongoCollection<SocialDocument> _users =
        database.GetCollection<SocialDocument>("social");

    private readonly IMongoCollection<FriendRequestDocument> _requests =
        database.GetCollection<FriendRequestDocument>(FriendRequestDocument.CollectionName);

    private readonly IMongoCollection<PartyInviteDocument> _invites =
        database.GetCollection<PartyInviteDocument>(PartyInviteDocument.CollectionName);

    public async Task EnsureUserAsync(string userId)
    {
        var update = Builders<SocialDocument>.Update
            .SetOnInsert(x => x.UserId, userId)
            .SetOnInsert(x => x.Friends, new List<string>())
            .SetOnInsert(x => x.Blocks, new List<string>())
            .SetOnInsert(x => x.FriendRequests, new List<FriendRequest>())
            .SetOnInsert(x => x.PartyInvites, new List<PartyInvite>());

        await _users.UpdateOneAsync(
            x => x.UserId == userId,
            update,
            new UpdateOptions { IsUpsert = true });
    }

    public async Task<bool> AreFriendsAsync(string userA, string userB)
    {
        var doc = await _users.Find(x => x.UserId == userA).FirstOrDefaultAsync();
        return doc is not null && doc.Friends.Contains(userB);
    }

    public async Task AddFriendAsync(string userA, string userB)
    {
        await EnsureUserAsync(userA);
        await EnsureUserAsync(userB);

        // Two writes. A retry is safe because $addToSet does not duplicate an id.
        // If the process dies after the first write, the friendship is one-sided until a later retry.
        await _users.UpdateOneAsync(
            x => x.UserId == userA,
            Builders<SocialDocument>.Update.AddToSet(x => x.Friends, userB));
        await _users.UpdateOneAsync(
            x => x.UserId == userB,
            Builders<SocialDocument>.Update.AddToSet(x => x.Friends, userA));
    }

    public async Task RemoveFriendAsync(string userA, string userB)
    {
        await EnsureUserAsync(userA);
        await EnsureUserAsync(userB);

        await _users.UpdateOneAsync(
            x => x.UserId == userA,
            Builders<SocialDocument>.Update.Pull(x => x.Friends, userB));
        await _users.UpdateOneAsync(
            x => x.UserId == userB,
            Builders<SocialDocument>.Update.Pull(x => x.Friends, userA));
    }

    public async Task<IReadOnlyList<string>> GetFriendsAsync(string userId)
    {
        var doc = await _users.Find(x => x.UserId == userId).FirstOrDefaultAsync();
        return doc?.Friends ?? [];
    }

    public async Task<bool> IsBlockedAsync(string blockerId, string targetId)
    {
        var doc = await _users.Find(x => x.UserId == blockerId).FirstOrDefaultAsync();
        return doc is not null && doc.Blocks.Contains(targetId);
    }

    public async Task<bool> IsBlockedEitherWayAsync(string userA, string userB)
        => await IsBlockedAsync(userA, userB) || await IsBlockedAsync(userB, userA);

    public async Task AddBlockAsync(string blockerId, string targetId)
    {
        await EnsureUserAsync(blockerId);
        await _users.UpdateOneAsync(
            x => x.UserId == blockerId,
            Builders<SocialDocument>.Update.AddToSet(x => x.Blocks, targetId));
    }

    public async Task RemoveBlockAsync(string blockerId, string targetId)
    {
        await _users.UpdateOneAsync(
            x => x.UserId == blockerId,
            Builders<SocialDocument>.Update.Pull(x => x.Blocks, targetId));
    }

    public async Task<IReadOnlyList<string>> GetBlocksAsync(string userId)
    {
        var doc = await _users.Find(x => x.UserId == userId).FirstOrDefaultAsync();
        return doc?.Blocks ?? [];
    }

    public async Task<FriendRequestDocument?> FindPendingFriendRequestAsync(string senderId, string receiverId)
    {
        await ExpireDueAsync(DateTime.UtcNow);
        return await _requests.Find(x =>
                x.SenderId == senderId &&
                x.ReceiverId == receiverId &&
                x.Status == FriendRequestStatus.Pending)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> HasPendingFriendRequestEitherWayAsync(string userA, string userB)
    {
        await ExpireDueAsync(DateTime.UtcNow);
        var filter = PendingFriendPair(userA, userB);
        return await _requests.Find(filter).AnyAsync();
    }

    public async Task<IReadOnlyList<FriendRequestDocument>> ListPendingFriendRequestsAsync(string userId)
    {
        await ExpireDueAsync(DateTime.UtcNow);
        var filter = Builders<FriendRequestDocument>.Filter.And(
            Builders<FriendRequestDocument>.Filter.Eq(x => x.Status, FriendRequestStatus.Pending),
            Builders<FriendRequestDocument>.Filter.Or(
                Builders<FriendRequestDocument>.Filter.Eq(x => x.SenderId, userId),
                Builders<FriendRequestDocument>.Filter.Eq(x => x.ReceiverId, userId)));
        return await _requests.Find(filter).ToListAsync();
    }

    public async Task AddFriendRequestAsync(FriendRequestDocument request)
    {
        try
        {
            await _requests.InsertOneAsync(request);
        }
        catch (MongoWriteException ex) when (ex.WriteError.Code == 11000)
        {
            throw new SocialConflictException("social.request_exists", "A pending friend request already exists.");
        }
    }

    public Task UpdateFriendRequestStatusAsync(Guid id, FriendRequestStatus status, DateTime respondedAt)
    {
        var update = Builders<FriendRequestDocument>.Update
            .Set(x => x.Status, status)
            .Set(x => x.RespondedAt, respondedAt);
        return _requests.UpdateOneAsync(x => x.Id == id && x.Status == FriendRequestStatus.Pending, update);
    }

    public Task VoidPendingFriendRequestsBetweenAsync(string userA, string userB)
    {
        var update = Builders<FriendRequestDocument>.Update
            .Set(x => x.Status, FriendRequestStatus.Cancelled)
            .Set(x => x.RespondedAt, DateTime.UtcNow);
        return _requests.UpdateManyAsync(PendingFriendPair(userA, userB), update);
    }

    public async Task<PartyInviteDocument?> FindPendingPartyInviteAsync(string senderId, string receiverId)
    {
        await ExpireDueAsync(DateTime.UtcNow);
        return await _invites.Find(x =>
                x.SenderId == senderId &&
                x.ReceiverId == receiverId &&
                x.Status == PartyInviteStatus.Pending)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> HasPendingPartyInviteEitherWayAsync(string userA, string userB)
    {
        await ExpireDueAsync(DateTime.UtcNow);
        return await _invites.Find(PendingPartyPair(userA, userB)).AnyAsync();
    }

    public async Task<IReadOnlyList<PartyInviteDocument>> ListPendingPartyInvitesAsync(string userId)
    {
        await ExpireDueAsync(DateTime.UtcNow);
        var filter = Builders<PartyInviteDocument>.Filter.And(
            Builders<PartyInviteDocument>.Filter.Eq(x => x.Status, PartyInviteStatus.Pending),
            Builders<PartyInviteDocument>.Filter.Or(
                Builders<PartyInviteDocument>.Filter.Eq(x => x.SenderId, userId),
                Builders<PartyInviteDocument>.Filter.Eq(x => x.ReceiverId, userId)));
        return await _invites.Find(filter).ToListAsync();
    }

    public async Task AddPartyInviteAsync(PartyInviteDocument invite)
    {
        try
        {
            await _invites.InsertOneAsync(invite);
        }
        catch (MongoWriteException ex) when (ex.WriteError.Code == 11000)
        {
            throw new SocialConflictException("social.invite_exists", "A pending party invite already exists.");
        }
    }

    public Task UpdatePartyInviteStatusAsync(Guid id, PartyInviteStatus status, DateTime respondedAt)
    {
        var update = Builders<PartyInviteDocument>.Update
            .Set(x => x.Status, status)
            .Set(x => x.RespondedAt, respondedAt);
        return _invites.UpdateOneAsync(x => x.Id == id && x.Status == PartyInviteStatus.Pending, update);
    }

    public Task VoidPendingPartyInvitesBetweenAsync(string userA, string userB)
    {
        var update = Builders<PartyInviteDocument>.Update
            .Set(x => x.Status, PartyInviteStatus.Cancelled)
            .Set(x => x.RespondedAt, DateTime.UtcNow);
        return _invites.UpdateManyAsync(PendingPartyPair(userA, userB), update);
    }

    public async Task ExpireDueAsync(DateTime utcNow)
    {
        var requestFilter = Builders<FriendRequestDocument>.Filter.And(
            Builders<FriendRequestDocument>.Filter.Eq(x => x.Status, FriendRequestStatus.Pending),
            Builders<FriendRequestDocument>.Filter.Lt(x => x.ExpiresAt, utcNow));
        var requestUpdate = Builders<FriendRequestDocument>.Update
            .Set(x => x.Status, FriendRequestStatus.Expired)
            .Set(x => x.RespondedAt, utcNow);
        await _requests.UpdateManyAsync(requestFilter, requestUpdate);

        var inviteFilter = Builders<PartyInviteDocument>.Filter.And(
            Builders<PartyInviteDocument>.Filter.Eq(x => x.Status, PartyInviteStatus.Pending),
            Builders<PartyInviteDocument>.Filter.Lt(x => x.ExpiresAt, utcNow));
        var inviteUpdate = Builders<PartyInviteDocument>.Update
            .Set(x => x.Status, PartyInviteStatus.Expired)
            .Set(x => x.RespondedAt, utcNow);
        await _invites.UpdateManyAsync(inviteFilter, inviteUpdate);
    }

    private static FilterDefinition<FriendRequestDocument> PendingFriendPair(string userA, string userB)
    {
        return Builders<FriendRequestDocument>.Filter.And(
            Builders<FriendRequestDocument>.Filter.Eq(x => x.Status, FriendRequestStatus.Pending),
            Builders<FriendRequestDocument>.Filter.Or(
                Builders<FriendRequestDocument>.Filter.And(
                    Builders<FriendRequestDocument>.Filter.Eq(x => x.SenderId, userA),
                    Builders<FriendRequestDocument>.Filter.Eq(x => x.ReceiverId, userB)),
                Builders<FriendRequestDocument>.Filter.And(
                    Builders<FriendRequestDocument>.Filter.Eq(x => x.SenderId, userB),
                    Builders<FriendRequestDocument>.Filter.Eq(x => x.ReceiverId, userA))));
    }

    private static FilterDefinition<PartyInviteDocument> PendingPartyPair(string userA, string userB)
    {
        return Builders<PartyInviteDocument>.Filter.And(
            Builders<PartyInviteDocument>.Filter.Eq(x => x.Status, PartyInviteStatus.Pending),
            Builders<PartyInviteDocument>.Filter.Or(
                Builders<PartyInviteDocument>.Filter.And(
                    Builders<PartyInviteDocument>.Filter.Eq(x => x.SenderId, userA),
                    Builders<PartyInviteDocument>.Filter.Eq(x => x.ReceiverId, userB)),
                Builders<PartyInviteDocument>.Filter.And(
                    Builders<PartyInviteDocument>.Filter.Eq(x => x.SenderId, userB),
                    Builders<PartyInviteDocument>.Filter.Eq(x => x.ReceiverId, userA))));
    }
}
