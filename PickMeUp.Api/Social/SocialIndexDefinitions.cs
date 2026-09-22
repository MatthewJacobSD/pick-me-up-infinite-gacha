using MongoDB.Driver;

namespace PickMeUp.Api.Social;

public static class SocialIndexDefinitions
{
    public static readonly TimeSpan RequestLifetime = TimeSpan.FromDays(7);

    // UserId is the social document _id, so the collection's _id index is the unique UserId index.
    public static CreateIndexModel<SocialDocument> UserIdUnique()
    {
        var keys = Builders<SocialDocument>.IndexKeys.Ascending(x => x.UserId);
        return new CreateIndexModel<SocialDocument>(keys, new CreateIndexOptions
        {
            Name = "userId_unique",
            Unique = true
        });
    }

    public static CreateIndexModel<FriendRequestDocument> PendingFriendPairUnique()
    {
        var keys = Builders<FriendRequestDocument>.IndexKeys
            .Ascending(x => x.SenderId)
            .Ascending(x => x.ReceiverId);
        return new CreateIndexModel<FriendRequestDocument>(keys, new CreateIndexOptions<FriendRequestDocument>
        {
            Name = "friend_requests_pending_pair",
            Unique = true,
            PartialFilterExpression = Builders<FriendRequestDocument>.Filter
                .Eq(x => x.Status, FriendRequestStatus.Pending)
        });
    }

    public static CreateIndexModel<PartyInviteDocument> PendingPartyPairUnique()
    {
        var keys = Builders<PartyInviteDocument>.IndexKeys
            .Ascending(x => x.SenderId)
            .Ascending(x => x.ReceiverId);
        return new CreateIndexModel<PartyInviteDocument>(keys, new CreateIndexOptions<PartyInviteDocument>
        {
            Name = "party_invites_pending_pair",
            Unique = true,
            PartialFilterExpression = Builders<PartyInviteDocument>.Filter
                .Eq(x => x.Status, PartyInviteStatus.Pending)
        });
    }
}
