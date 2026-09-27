using MongoDB.Driver;
using PickMeUp.Api.Common.Errors;
using PickMeUp.Api.Account.Authentication.OAuth;

namespace PickMeUp.Api.Account.Authentication.OAuth;

/**--------[MongoDB Repositories]--------**/

/// <summary>
/// MongoDB implementation of <see cref="IAccountRepository"/>.
/// Only Active accounts are returned by default queries.
/// Soft-deleted accounts live in a separate <c>accounts_deleted</c> collection.
/// </summary>
public sealed class MongoAccountRepository(IMongoDatabase db) : IAccountRepository
{
    private readonly IMongoCollection<Account> _collection = db.GetCollection<Account>("accounts");
    private readonly IMongoCollection<Account> _deletedCollection = db.GetCollection<Account>("accounts_deleted");

    public Account? FindByEmail(string email)
        => _collection.Find(x => x.Email == email && x.Status == AccountStatus.Active).FirstOrDefault();

    public Account? FindByPublicCode(string publicCode)
        => _collection.Find(x => x.PublicCode == publicCode && x.Status == AccountStatus.Active).FirstOrDefault();

    public Account? FindDeletedByEmail(string email)
        => _deletedCollection.Find(x => x.Email == email).FirstOrDefault();

    public Account Create(Account account)
    {
        _collection.InsertOne(account);
        return account;
    }

    public async Task SoftDeleteAsync(Guid accountId)
    {
        var filter = Builders<Account>.Filter.Eq(x => x.Id, accountId);
        var account = _collection.FindOneAndDelete(filter);
        if (account is not null)
        {
            var deletedAccount = new Account
            {
                Id = account.Id,
                PublicCode = account.PublicCode,
                Email = account.Email,
                GameEmail = account.GameEmail,
                Username = account.Username,
                AvatarUrl = account.AvatarUrl,
                Status = AccountStatus.SoftDeleted,
                CreatedAt = account.CreatedAt,
                DeletedAt = DateTime.UtcNow,
                PurgeAt = DateTime.UtcNow.AddDays(30)
            };
            _deletedCollection.InsertOne(deletedAccount);
        }
    }

    public async Task<Account> RecoverAsync(string email)
    {
        var filter = Builders<Account>.Filter.Eq(x => x.Email, email);
        var account = _deletedCollection.FindOneAndDelete(filter);
        if (account is null)
            throw new NotFoundException("No deleted account found.");

        var recoveredAccount = new Account
        {
            Id = account.Id,
            PublicCode = account.PublicCode,
            Email = account.Email,
            GameEmail = account.GameEmail,
            Username = account.Username,
            AvatarUrl = account.AvatarUrl,
            Status = AccountStatus.Active,
            CreatedAt = account.CreatedAt,
            DeletedAt = null,
            PurgeAt = null
        };

        _collection.InsertOne(recoveredAccount);
        return recoveredAccount;
    }

    public void PurgeExpiredAsync()
    {
        var filter = Builders<Account>.Filter.Lte(x => x.PurgeAt, DateTime.UtcNow);
        _deletedCollection.DeleteMany(filter);
    }
}

/// <summary>
/// MongoDB implementation of <see cref="IExternalAccountRepository"/>.
/// </summary>
public sealed class MongoExternalAccountRepository(IMongoDatabase db) : IExternalAccountRepository
{
    private readonly IMongoCollection<ExternalAccountLink> _collection =
        db.GetCollection<ExternalAccountLink>("oauth_identities");

    public ExternalAccountLink? FindByExternalId(OAuthProvider provider, string externalId)
        => _collection.Find(x => x.Provider == provider && x.ExternalId == externalId).FirstOrDefault();

    public ExternalAccountLink? FindByEmail(string email)
        => _collection.Find(x => x.Email == email).FirstOrDefault();

    public void Add(ExternalAccountLink link)
        => _collection.InsertOne(link);
}
