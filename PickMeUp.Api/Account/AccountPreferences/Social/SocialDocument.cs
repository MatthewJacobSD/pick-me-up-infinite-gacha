using MongoDB.Bson.Serialization.Attributes;

namespace PickMeUp.Api.Account.AccountPreferences.Social
{
    // ── Social Document ────────────────────────────────
    // MongoDB document for a player's social state.
    // One document per user in the "social" collection.

    public sealed class SocialDocument
    {
        [BsonId]
        public string UserId { get; init; } = string.Empty;

        public List<string> Friends { get; init; } = [];
        public List<string> Blocks { get; init; } = [];

        public List<FriendRequest> FriendRequests { get; init; } = [];
        public List<PartyInvite> PartyInvites { get; init; } = [];
    }
}
