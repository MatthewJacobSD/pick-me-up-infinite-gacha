namespace PickMeUp.Api.Account.Profile;

public sealed class ProfileDto
{
    public Guid AccountId { get; init; }
    public string Username { get; init; } = string.Empty;
    public string AvatarValue { get; init; } = string.Empty;
    public string AvatarUrlPath { get; init; } = string.Empty;
    public string AvatarType { get; init; } = string.Empty;
    public bool AvatarIsDefault { get; init; }
    public int Version { get; init; }

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
