namespace PickMeUp.Api.Account.Profile;

public sealed class UpdateUsernameDto
{
    public string Username { get; init; } = string.Empty;
    public int Version { get; init; }
}
