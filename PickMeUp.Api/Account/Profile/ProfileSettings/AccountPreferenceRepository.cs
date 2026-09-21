using MongoDB.Driver;

namespace PickMeUp.Api.Account.Profile.ProfileSettings
{
    // MongoDB implementation of IAccountPreferencesRepository.
    // One document per user in the "account_preferences" collection.
    // Each update increments the Version field for optimistic concurrency.

    public sealed class AccountPreferencesRepository(IMongoDatabase db) : IAccountPreferencesRepository
    {
        private readonly IMongoCollection<AccountPreferencesDocument> _collection =
            db.GetCollection<AccountPreferencesDocument>("account_preferences");

        // Returns existing document or creates a new one with defaults.
        private async Task<AccountPreferencesDocument> GetOrCreateAsync(string userId)
        {
            var doc = await _collection.Find(x => x.UserId == userId).FirstOrDefaultAsync();

            if (doc is null)
            {
                doc = new AccountPreferencesDocument { UserId = userId };
                await _collection.InsertOneAsync(doc);
            }

            return doc;
        }

        public Task<AccountPreferencesDocument> GetAsync(string userId)
            => GetOrCreateAsync(userId);

        // ── Gameplay ──────────────────────────────────────────────

        public async Task<Gameplay.GameplaySettings> GetGameplaySettingsAsync(string userId)
        {
            var doc = await GetOrCreateAsync(userId);
            return doc.Gameplay;
        }

        public async Task UpdateGameplaySettingsAsync(string userId, Gameplay.GameplaySettings settings)
        {
            var update = Builders<AccountPreferencesDocument>.Update
                .Set(x => x.Gameplay, settings)
                .Inc(x => x.Version, 1);

            await _collection.UpdateOneAsync(x => x.UserId == userId, update);
        }

        // ── Accessibility ─────────────────────────────────────────

        public async Task<Accessibility.AccessibilitySettings> GetAccessibilitySettingsAsync(string userId)
        {
            var doc = await GetOrCreateAsync(userId);
            return doc.Accessibility;
        }

        public async Task UpdateAccessibilitySettingsAsync(string userId, Accessibility.AccessibilitySettings settings)
        {
            var update = Builders<AccountPreferencesDocument>.Update
                .Set(x => x.Accessibility, settings)
                .Inc(x => x.Version, 1);

            await _collection.UpdateOneAsync(x => x.UserId == userId, update);
        }
    }
}
