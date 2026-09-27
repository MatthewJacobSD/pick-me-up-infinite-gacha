using System.Linq.Expressions;
using MongoDB.Driver;
using PickMeUp.Api.Common.Errors;

namespace PickMeUp.Api.Account.Profile;

/**--------[Repository Implementation]--------**/

/// <summary>
/// MongoDB-backed implementation of <see cref="IProfileRepository"/>.
/// Uses atomic upserts and version-gated replacements for safe concurrent access.
/// </summary>
public sealed class ProfileRepository(IMongoDatabase db) : IProfileRepository
{
    private readonly IMongoCollection<ProfileDocument> _collection =
        db.GetCollection<ProfileDocument>("profile");

    /// <summary>
    /// Atomically upserts a profile document. On first call for an account all fields
    /// are written via <c>SetOnInsert</c>; subsequent calls return the existing document.
    /// </summary>
    public async Task<ProfileDocument> GetOrCreateAsync(Guid accountId)
    {
        var defaultUsername = Username.Create("unnamed");
        var defaultAvatar = Avatar.Create(string.Empty, "/avatars/default.png");

        // SetOnInsert — values only take effect when a new document is created.
        var update = Builders<ProfileDocument>.Update
            .SetOnInsert(x => x.AccountId, accountId)
            .SetOnInsert(x => x.Username, defaultUsername)
            .SetOnInsert(x => x.Avatar, defaultAvatar)
            .SetOnInsert(x => x.Version, 1);

        await _collection.UpdateOneAsync(
            x => x.AccountId == accountId,
            update,
            new UpdateOptions { IsUpsert = true });

        // Read back to return the full document (upsert doesn't return it by default).
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

    /// <summary>
    /// Generic single-field update with optimistic concurrency.
    /// The MongoDB filter includes <c>Version == expectedVersion</c> so the write
    /// succeeds only if no other writer has bumped the version in the meantime.
    /// </summary>
    private async Task<ProfileDocument> ReplaceSliceAsync<TField>(
        Guid accountId,
        Expression<Func<ProfileDocument, TField>> field,
        TField value,
        int expectedVersion)
    {
        var update = Builders<ProfileDocument>.Update
            .Set(field, value)
            .Inc(x => x.Version, 1);

        // FindOneAndUpdate returns null when the filter matches nothing (version mismatch).
        var result = await _collection.FindOneAndUpdateAsync(
            x => x.AccountId == accountId && x.Version == expectedVersion,
            update, new FindOneAndUpdateOptions<ProfileDocument>
            {
                ReturnDocument = ReturnDocument.After
            }) ?? throw new VersionConflictException();
        return result;
    }
}
