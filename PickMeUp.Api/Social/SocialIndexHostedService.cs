using MongoDB.Driver;

namespace PickMeUp.Api.Social;

public sealed class SocialIndexHostedService(IMongoDatabase database) : IHostedService
{
    private readonly IMongoDatabase _database = database;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var users = _database.GetCollection<SocialDocument>("social");
        try
        {
            await users.Indexes.CreateOneAsync(SocialIndexDefinitions.UserIdUnique(), cancellationToken: cancellationToken);
        }
        catch (MongoCommandException)
        {
            // UserId is the document _id. Mongo already keeps that index unique.
        }

        var requests = _database.GetCollection<FriendRequestDocument>(FriendRequestDocument.CollectionName);
        await requests.Indexes.CreateOneAsync(
            SocialIndexDefinitions.PendingFriendPairUnique(),
            cancellationToken: cancellationToken);

        var invites = _database.GetCollection<PartyInviteDocument>(PartyInviteDocument.CollectionName);
        await invites.Indexes.CreateOneAsync(
            SocialIndexDefinitions.PendingPartyPairUnique(),
            cancellationToken: cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
