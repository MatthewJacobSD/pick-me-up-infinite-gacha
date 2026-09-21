using MongoDB.Driver;

namespace PickMeUp.Api.Account.Profile.ProfileSettings.Social
{
    // ── Social Repository ──────────────────────────────
    // MongoDB implementation of ISocialRepository.
    // Each user has one SocialDocument in the "social" collection.
    // Friend/block operations are bidirectional (update both users).

    public sealed class SocialRepository(IMongoDatabase db) : ISocialRepository
    {
        private readonly IMongoCollection<SocialDocument> _collection = db.GetCollection<SocialDocument>("social");

        // 1. Look up existing document or create a new one.
        private async Task<SocialDocument> GetOrCreateAsync(string userId)
        {
            var doc = await _collection.Find(x => x.UserId == userId).FirstOrDefaultAsync();

            if (doc is null)
            {
                doc = new SocialDocument { UserId = userId };
                await _collection.InsertOneAsync(doc);
            }

            return doc;
        }

        // ── Friends ────────────────────────────────────

        public async Task<bool> AreFriendsAsync(string userA, string userB)
        {
            var doc = await GetOrCreateAsync(userA);
            return doc.Friends.Contains(userB);
        }

        // Bidirectional — adds to both users' friend lists.
        public async Task AddFriendAsync(string userA, string userB)
        {
            await _collection.UpdateOneAsync(
                x => x.UserId == userA,
                Builders<SocialDocument>.Update.AddToSet(x => x.Friends, userB));

            await _collection.UpdateOneAsync(
                x => x.UserId == userB,
                Builders<SocialDocument>.Update.AddToSet(x => x.Friends, userA));
        }

        // Bidirectional — removes from both users' friend lists.
        public async Task RemoveFriendAsync(string userA, string userB)
        {
            await _collection.UpdateOneAsync(
                x => x.UserId == userA,
                Builders<SocialDocument>.Update.Pull(x => x.Friends, userB));

            await _collection.UpdateOneAsync(
                x => x.UserId == userB,
                Builders<SocialDocument>.Update.Pull(x => x.Friends, userA));
        }

        public async Task<IReadOnlyList<string>> GetFriendsAsync(string userId)
        {
            var doc = await GetOrCreateAsync(userId);
            return doc.Friends;
        }

        // ── Blocks ─────────────────────────────────────

        public async Task<bool> IsBlockedAsync(string blockerId, string targetId)
        {
            var doc = await GetOrCreateAsync(blockerId);
            return doc.Blocks.Contains(targetId);
        }

        // Unidirectional — only the blocker's list is updated.
        public async Task AddBlockAsync(string blockerId, string targetId)
        {
            await _collection.UpdateOneAsync(
                x => x.UserId == blockerId,
                Builders<SocialDocument>.Update.AddToSet(x => x.Blocks, targetId));
        }

        public async Task RemoveBlockAsync(string blockerId, string targetId)
        {
            await _collection.UpdateOneAsync(
                x => x.UserId == blockerId,
                Builders<SocialDocument>.Update.Pull(x => x.Blocks, targetId));
        }

        public async Task<IReadOnlyList<string>> GetBlocksAsync(string userId)
        {
            var doc = await GetOrCreateAsync(userId);
            return doc.Blocks;
        }

        // ── Friend Requests ────────────────────────────

        public async Task<bool> HasPendingRequestAsync(string senderId, string receiverId)
        {
            var doc = await GetOrCreateAsync(receiverId);
            return doc.FriendRequests.Any(r =>
                r.FromUserId == senderId &&
                r.ToUserId == receiverId &&
                r.Status == FriendRequestStatus.Pending);
        }

        public async Task<FriendRequest?> GetFriendRequestAsync(string senderId, string receiverId)
        {
            var doc = await GetOrCreateAsync(receiverId);
            return doc.FriendRequests.FirstOrDefault(r =>
                r.FromUserId == senderId &&
                r.ToUserId == receiverId);
        }

        public async Task AddFriendRequestAsync(FriendRequest request)
        {
            await _collection.UpdateOneAsync(
                x => x.UserId == request.ToUserId,
                Builders<SocialDocument>.Update.AddToSet(x => x.FriendRequests, request));
        }

        // 1. Filter to receiver's document.
        // 2. Match the specific request by sender/receiver IDs.
        // 3. Set status via positional operator.
        public async Task UpdateFriendRequestStatusAsync(string senderId, string receiverId, FriendRequestStatus status)
        {
            var filter = Builders<SocialDocument>.Filter.And(
                Builders<SocialDocument>.Filter.Eq(x => x.UserId, receiverId),
                Builders<SocialDocument>.Filter.ElemMatch(x => x.FriendRequests,
                    r => r.FromUserId == senderId && r.ToUserId == receiverId)
            );

            var update = Builders<SocialDocument>.Update.Set("FriendRequests.$.Status", status);

            await _collection.UpdateOneAsync(filter, update);
        }

        // ── Party Invites ──────────────────────────────

        public async Task AddPartyInviteAsync(PartyInvite invite)
        {
            await _collection.UpdateOneAsync(
                x => x.UserId == invite.ToUserId,
                Builders<SocialDocument>.Update.AddToSet(x => x.PartyInvites, invite));
        }

        public async Task<PartyInvite?> GetPartyInviteAsync(string senderId, string receiverId)
        {
            var doc = await GetOrCreateAsync(receiverId);
            return doc.PartyInvites.FirstOrDefault(r =>
                r.FromUserId == senderId &&
                r.ToUserId == receiverId);
        }

        // 1. Filter to receiver's document.
        // 2. Match the specific invite by sender/receiver IDs.
        // 3. Set status via positional operator.
        public async Task UpdatePartyInviteStatusAsync(string senderId, string receiverId, PartyInviteStatus status)
        {
            var filter = Builders<SocialDocument>.Filter.And(
                Builders<SocialDocument>.Filter.Eq(x => x.UserId, receiverId),
                Builders<SocialDocument>.Filter.ElemMatch(x => x.PartyInvites,
                    r => r.FromUserId == senderId && r.ToUserId == receiverId)
            );

            var update = Builders<SocialDocument>.Update.Set("PartyInvites.$.Status", status);

            await _collection.UpdateOneAsync(filter, update);
        }
    }
}
