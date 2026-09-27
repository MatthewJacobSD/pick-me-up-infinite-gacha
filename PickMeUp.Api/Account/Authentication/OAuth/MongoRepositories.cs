using MongoDB.Driver;
using PickMeUp.Api.Account.Authentication.OAuth;

namespace PickMeUp.Api.Account.Authentication.OAuth;

// Minimal MongoDB implementation of IAccountRepository for OAuth account creation.
public sealed class MongoAccountRepository(IMongoDatabase db) : IAccountRepository
{
    private readonly IMongoCollection<Account> _collection = db.GetCollection<Account>("accounts");

    public Account? FindByEmail(string email)
        => _collection.Find(x => x.Email == email).FirstOrDefault();

    public Account? FindByPersonalId(string personalId)
        => _collection.Find(x => x.PersonalId == personalId).FirstOrDefault();

    public Account Create(Account account)
    {
        _collection.InsertOne(account);
        return account;
    }
}

// Minimal MongoDB implementation of IExternalAccountRepository for OAuth linking.
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
