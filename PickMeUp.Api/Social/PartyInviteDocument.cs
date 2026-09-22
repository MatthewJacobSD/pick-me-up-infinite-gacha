using MongoDB.Bson.Serialization.Attributes;

namespace PickMeUp.Api.Social;

public sealed class PartyInviteDocument
{
    public const string CollectionName = "party_invites";

    [BsonId]
    public Guid Id { get; set; }

    public string SenderId { get; set; } = string.Empty;
    public string ReceiverId { get; set; } = string.Empty;
    public PartyInviteStatus Status { get; set; } = PartyInviteStatus.Pending;
    public DateTime CreatedAt { get; set; }
    public DateTime? RespondedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}
