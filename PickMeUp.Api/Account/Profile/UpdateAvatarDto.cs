namespace PickMeUp.Api.Account.Profile;

/// <summary>
/// Request body for <c>PUT /account/profile/avatar</c>.
/// </summary>
public sealed class UpdateAvatarDto
{
    public string Value { get; init; } = string.Empty;
    public string AvatarUrlPath { get; init; } = string.Empty;
    public Avatar.AvatarTypeStatus AvatarType { get; init; } = Avatar.AvatarTypeStatus.Default;
    /// <summary>Optimistic concurrency token — must match the profile's current version.</summary>
    public int Version { get; init; }
}
