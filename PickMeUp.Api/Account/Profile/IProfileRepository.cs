namespace PickMeUp.Api.Account.Profile;

public interface IProfileRepository
{
    Task<ProfileDocument> GetOrCreateAsync(Guid accountId);
    Task<ProfileDocument?> FindAsync(Guid accountId);
    Task<ProfileDocument> UpdateUsernameAsync(Guid accountId, Username username, int expectedVersion);
    Task<ProfileDocument> UpdateAvatarAsync(Guid accountId, Avatar avatar, int expectedVersion);
}
