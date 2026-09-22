using MongoDB.Bson.Serialization.Attributes;

namespace PickMeUp.Api.Social;

// Flat friend-request row. Pending pairs are unique so both incoming and outgoing
// reads stay a single collection query.
public sealed class FriendRequestDocument
{
    public const string CollectionName = "friend_requests";

    [BsonId]
    public Guid Id { get; set; }

    public string SenderId { get; set; } = string.Empty;
    public string ReceiverId { get; set; } = string.Empty;
    public FriendRequestStatus Status { get; set; } = FriendRequestStatus.Pending;
    public DateTime CreatedAt { get; set; }
    public DateTime? RespondedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}
