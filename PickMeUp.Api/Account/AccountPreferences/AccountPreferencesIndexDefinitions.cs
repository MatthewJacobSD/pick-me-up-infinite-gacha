using MongoDB.Driver;

namespace PickMeUp.Api.Account.AccountPreferences;

/// <summary>
/// MongoDB index definitions for the account-preferences collection.
/// </summary>
public static class AccountPreferencesIndexDefinitions
{
    /// <summary>
    /// Unique index on AccountId to enforce one preference document per account.
    /// </summary>
    public static CreateIndexModel<AccountDocument> AccountIdUnique()
    {
        var keys = Builders<AccountDocument>.IndexKeys.Ascending(x => x.AccountId);
        return new CreateIndexModel<AccountDocument>(keys, new CreateIndexOptions
        {
            Name = "accountId_unique",
            Unique = true
        });
    }
}
