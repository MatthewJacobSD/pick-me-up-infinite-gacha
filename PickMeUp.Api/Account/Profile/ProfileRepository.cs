using System.Linq.Expressions;
using MongoDB.Driver;
using PickMeUp.Api.Common.Errors;

namespace PickMeUp.Api.Account.Profile;

public sealed class ProfileRepository(IMongoDatabase db) : IProfileRepository
{
    private readonly IMongoCollection<ProfileDocument> _collection =
        db.GetCollection<ProfileDocument>("profile");

    public async Task<ProfileDocument> GetOrCreateAsync(Guid accountId)
    {
        var defaultUsername = Username.Create("unnamed");
        var defaultAvatar = Avatar.Create(string.Empty, "/avatars/default.png");

        var update = Builders<ProfileDocument>.Update
            .SetOnInsert(x => x.AccountId, accountId)
            .SetOnInsert(x => x.Username, defaultUsername)
            .SetOnInsert(x => x.Avatar, defaultAvatar)
            .SetOnInsert(x => x.Version, 1);

        await _collection.UpdateOneAsync(
            x => x.AccountId == accountId,
            update,
            new UpdateOptions { IsUpsert = true });

        return (await _collection.Find(x => x.AccountId == accountId).FirstOrDefaultAsync())!;
    }

    public async Task<ProfileDocument?> FindAsync(Guid accountId)
        => await _collection.Find(x => x.AccountId == accountId).FirstOrDefaultAsync();

    public Task<ProfileDocument> UpdateUsernameAsync(
        Guid accountId, Username username, int expectedVersion)
        => ReplaceSliceAsync(accountId, x => x.Username, username, expectedVersion);

    public Task<ProfileDocument> UpdateAvatarAsync(
        Guid accountId, Avatar avatar, int expectedVersion)
        => ReplaceSliceAsync(accountId, x => x.Avatar, avatar, expectedVersion);

    private async Task<ProfileDocument> ReplaceSliceAsync<TField>(
        Guid accountId,
        Expression<Func<ProfileDocument, TField>> field,
        TField value,
        int expectedVersion)
    {
        var update = Builders<ProfileDocument>.Update
            .Set(field, value)
            .Inc(x => x.Version, 1);

        var result = await _collection.FindOneAndUpdateAsync(
            x => x.AccountId == accountId && x.Version == expectedVersion,
            update, new FindOneAndUpdateOptions<ProfileDocument>
            {
                ReturnDocument = ReturnDocument.After
            }) ?? throw new VersionConflictException();
        return result;
    }
}
