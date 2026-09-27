using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PickMeUp.Api.Account.Profile;

/**--------[Document Model]--------**/

/// <summary>
/// MongoDB document mapping for a player profile.
/// Each field uses an immutable value type (<see cref="Username"/>, <see cref="Avatar"/>)
/// so partial updates are expressed as atomic field replacements.
/// </summary>
public sealed class ProfileDocument
{
    /// <summary>Primary key — matches the authentication account ID.</summary>
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid AccountId { get; init; }

    /// <summary>Player-visible display name (value object with validation).</summary>
    public Username Username { get; init; } = Username.Create("unnamed");

    /// <summary>Current avatar selection (value object wrapping path + type).</summary>
    public Avatar Avatar { get; init; } = Avatar.Create(string.Empty, "/avatars/default.png");

    /// <summary>Monotonically increasing version for optimistic concurrency control.</summary>
    public int Version { get; init; } = 1;
}
