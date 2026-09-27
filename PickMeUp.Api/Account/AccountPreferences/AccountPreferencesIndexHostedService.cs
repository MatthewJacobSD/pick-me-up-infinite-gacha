using MongoDB.Driver;

namespace PickMeUp.Api.Account.AccountPreferences;

/// <summary>
/// Ensures MongoDB indexes for account-preferences collections exist at startup.
/// </summary>
public sealed class AccountPreferencesIndexHostedService(IMongoDatabase database) : IHostedService
{
    private readonly IMongoDatabase _database = database;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var collection = _database.GetCollection<AccountDocument>("account_preferences");
        await collection.Indexes.CreateOneAsync(
            AccountPreferencesIndexDefinitions.AccountIdUnique(),
            cancellationToken: cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
