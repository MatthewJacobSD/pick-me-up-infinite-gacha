namespace PickMeUp.Api.Account.Profile;

public sealed class UpdateAvatarDto
{
    public string Value { get; init; } = string.Empty;
    public string AvatarUrlPath { get; init; } = string.Empty;
    public Avatar.AvatarTypeStatus AvatarType { get; init; } = Avatar.AvatarTypeStatus.Default;
    public int Version { get; init; }
}
