namespace PickMeUp.Api.Account.Profile;

/// <summary>
/// Request body for <c>PUT /account/profile/username</c>.
/// </summary>
public sealed class UpdateUsernameDto
{
    public string Username { get; init; } = string.Empty;
    /// <summary>Optimistic concurrency token — must match the profile's current version.</summary>
    public int Version { get; init; }
}
