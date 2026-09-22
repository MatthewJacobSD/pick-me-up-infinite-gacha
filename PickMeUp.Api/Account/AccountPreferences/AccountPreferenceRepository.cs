using MongoDB.Driver;

namespace PickMeUp.Api.Account.AccountPreferences
{
    // ── Account Preferences Repository ─────────────────
    // MongoDB implementation of IAccountPreferencesRepository.
    // One document per user in the "account_preferences" collection.
    // Each update increments the Version field for optimistic concurrency.

    public sealed class AccountPreferencesRepository(IMongoDatabase db) : IAccountPreferencesRepository
    {
        private readonly IMongoCollection<AccountPreferencesDocument> _collection =
            db.GetCollection<AccountPreferencesDocument>("account_preferences");

        // 1. Look up existing document or create a new one with defaults.
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

        // ── Gameplay ───────────────────────────────────

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

        // ── Accessibility ──────────────────────────────

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

        public async Task<Language.LanguageSettings> GetLanguageSettingsAsync(string userId)
        {
            var doc = await GetOrCreateAsync(userId);
            return doc.Language;
        }

        public async Task UpdateLanguageSettingsAsync(string userId, Language.LanguageSettings settings)
        {
            var update = Builders<AccountPreferencesDocument>.Update
                .Set(x => x.Language, settings)
                .Inc(x => x.Version, 1);

            await _collection.UpdateOneAsync(x => x.UserId == userId, update);
        }

        public async Task<Notifications.NotificationSettings> GetNotificationSettingsAsync(string userId)
        {
            var doc = await GetOrCreateAsync(userId);
            return doc.Notifications;
        }

        public async Task UpdateNotificationSettingsAsync(string userId, Notifications.NotificationSettings settings)
        {
            var update = Builders<AccountPreferencesDocument>.Update
                .Set(x => x.Notifications, settings)
                .Inc(x => x.Version, 1);

            await _collection.UpdateOneAsync(x => x.UserId == userId, update);
        }

        public async Task<Audio.AudioPreferencesSettings> GetAudioPreferencesAsync(string userId)
        {
            var doc = await GetOrCreateAsync(userId);
            return doc.Audio;
        }

        public async Task UpdateAudioPreferencesAsync(string userId, Audio.AudioPreferencesSettings settings)
        {
            var update = Builders<AccountPreferencesDocument>.Update
                .Set(x => x.Audio, settings)
                .Inc(x => x.Version, 1);

            await _collection.UpdateOneAsync(x => x.UserId == userId, update);
        }

        // ── SocialPreferences ──────────────────────────

        public async Task<SocialPreferences.SocialPreferencesSettings> GetSocialPreferencesAsync(string userId)
        {
            var doc = await GetOrCreateAsync(userId);
            return doc.SocialPreferences;
        }

        public async Task UpdateSocialPreferencesAsync(string userId, SocialPreferences.SocialPreferencesSettings settings)
        {
            var update = Builders<AccountPreferencesDocument>.Update
                .Set(x => x.SocialPreferences, settings)
                .Inc(x => x.Version, 1);

            await _collection.UpdateOneAsync(x => x.UserId == userId, update);
        }

        // ── UiPreferences ──────────────────────────────

        public async Task<UiPreferences.UiPreferencesSettings> GetUiPreferencesAsync(string userId)
        {
            var doc = await GetOrCreateAsync(userId);
            return doc.UiPreferences;
        }

        public async Task UpdateUiPreferencesAsync(string userId, UiPreferences.UiPreferencesSettings settings)
        {
            var update = Builders<AccountPreferencesDocument>.Update
                .Set(x => x.UiPreferences, settings)
                .Inc(x => x.Version, 1);

            await _collection.UpdateOneAsync(x => x.UserId == userId, update);
        }

    }
}
