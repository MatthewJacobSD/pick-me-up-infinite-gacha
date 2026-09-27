namespace PickMeUp.Api.Account.Profile;

/**--------[DTO]--------**/

/// <summary>
/// API response shape for a player profile.
/// Flattens the value-object hierarchy of <see cref="ProfileDocument"/> for client consumption.
/// </summary>
public sealed class ProfileDto
{
    public Guid AccountId { get; init; }
    public string Username { get; init; } = string.Empty;
    public string AvatarValue { get; init; } = string.Empty;
    public string AvatarUrlPath { get; init; } = string.Empty;
    public string AvatarType { get; init; } = string.Empty;
    public bool AvatarIsDefault { get; init; }
    /// <summary>Client must echo this back on write requests for optimistic concurrency.</summary>
    public int Version { get; init; }

    /// <summary>Projects a persistence document onto the API contract.</summary>
    public static ProfileDto FromDocument(ProfileDocument doc) => new()
    {
        AccountId = doc.AccountId,
        Username = doc.Username.Value,
        AvatarValue = doc.Avatar.Value,
        AvatarUrlPath = doc.Avatar.AvatarUrlPath,
        AvatarType = doc.Avatar.AvatarType.ToString(),
        AvatarIsDefault = doc.Avatar.IsDefault,
        Version = doc.Version,
    };
}
