using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PickMeUp.Api.Account.Profile;

public sealed class ProfileDocument
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid AccountId { get; init; }

    public Username Username { get; init; } = Username.Create("unnamed");

    public Avatar Avatar { get; init; } = Avatar.Create(string.Empty, "/avatars/default.png");

    public int Version { get; init; } = 1;
}
