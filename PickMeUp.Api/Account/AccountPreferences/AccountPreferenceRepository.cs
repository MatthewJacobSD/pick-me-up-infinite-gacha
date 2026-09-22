using System.Linq.Expressions;
using MongoDB.Driver;
using PickMeUp.Api.Account.AccountPreferences.SocialPreferences;
using PickMeUp.Api.Common.Errors;

namespace PickMeUp.Api.Account.AccountPreferences
{
    public sealed class AccountPreferencesRepository(IMongoDatabase db) : IAccountPreferencesRepository
    {
        private readonly IMongoCollection<AccountPreferencesDocument> _collection =
            db.GetCollection<AccountPreferencesDocument>("account_preferences");

        public async Task<AccountPreferencesDocument> GetOrCreateAsync(Guid accountId)
        {
            var update = Builders<AccountPreferencesDocument>.Update
                .SetOnInsert(x => x.AccountId, accountId)
                .SetOnInsert(x => x.UserId, accountId.ToString())
                .SetOnInsert(x => x.Gameplay, new Gameplay.GameplaySettings())
                .SetOnInsert(x => x.Accessibility, new Accessibility.AccessibilitySettings())
                .SetOnInsert(x => x.Language, new Language.LanguageSettings())
                .SetOnInsert(x => x.Notifications, new Notifications.NotificationSettings())
                .SetOnInsert(x => x.SocialPreferences, new SocialPreferencesSettings())
                .SetOnInsert(x => x.Audio, new Audio.AudioPreferencesSettings())
                .SetOnInsert(x => x.UiPreferences, new UiPreferences.UiPreferencesSettings())
                .SetOnInsert(x => x.Version, 1);

            await _collection.UpdateOneAsync(
                x => x.AccountId == accountId,
                update,
                new UpdateOptions { IsUpsert = true });

            return (await _collection.Find(x => x.AccountId == accountId).FirstOrDefaultAsync())!;
        }

        public async Task<AccountPreferencesDocument?> FindAsync(Guid accountId)
            => await _collection.Find(x => x.AccountId == accountId).FirstOrDefaultAsync();

        // ── Gameplay ───────────────────────────────────

        public async Task<Gameplay.GameplaySettings> GetGameplaySettingsAsync(Guid accountId)
        {
            var doc = await GetOrCreateAsync(accountId);
            return doc.Gameplay;
        }

        public Task<int> UpdateGameplaySettingsAsync(Guid accountId, Gameplay.GameplaySettings settings, int expectedVersion)
            => UpdateSliceAsync(accountId, x => x.Gameplay, settings, expectedVersion);

        // ── Accessibility ──────────────────────────────

        public async Task<Accessibility.AccessibilitySettings> GetAccessibilitySettingsAsync(Guid accountId)
        {
            var doc = await GetOrCreateAsync(accountId);
            return doc.Accessibility;
        }

        public Task<int> UpdateAccessibilitySettingsAsync(Guid accountId, Accessibility.AccessibilitySettings settings, int expectedVersion)
            => UpdateSliceAsync(accountId, x => x.Accessibility, settings, expectedVersion);

        // ── Language ───────────────────────────────────

        public async Task<Language.LanguageSettings> GetLanguageSettingsAsync(Guid accountId)
        {
            var doc = await GetOrCreateAsync(accountId);
            return doc.Language;
        }

        public Task<int> UpdateLanguageSettingsAsync(Guid accountId, Language.LanguageSettings settings, int expectedVersion)
            => UpdateSliceAsync(accountId, x => x.Language, settings, expectedVersion);

        // ── Notifications ──────────────────────────────

        public async Task<Notifications.NotificationSettings> GetNotificationSettingsAsync(Guid accountId)
        {
            var doc = await GetOrCreateAsync(accountId);
            return doc.Notifications;
        }

        public Task<int> UpdateNotificationSettingsAsync(Guid accountId, Notifications.NotificationSettings settings, int expectedVersion)
            => UpdateSliceAsync(accountId, x => x.Notifications, settings, expectedVersion);

        // ── SocialPreferences ──────────────────────────

        public async Task<SocialPreferencesSettings> GetSocialPreferencesAsync(Guid accountId)
        {
            var doc = await GetOrCreateAsync(accountId);
            return doc.SocialPreferences;
        }

        public Task<int> UpdateSocialPreferencesAsync(Guid accountId, SocialPreferencesSettings settings, int expectedVersion)
            => UpdateSliceAsync(accountId, x => x.SocialPreferences, settings, expectedVersion);

        // ── Audio ──────────────────────────────────────

        public async Task<Audio.AudioPreferencesSettings> GetAudioPreferencesAsync(Guid accountId)
        {
            var doc = await GetOrCreateAsync(accountId);
            return doc.Audio;
        }

        public Task<int> UpdateAudioPreferencesAsync(Guid accountId, Audio.AudioPreferencesSettings settings, int expectedVersion)
            => UpdateSliceAsync(accountId, x => x.Audio, settings, expectedVersion);

        // ── UiPreferences ──────────────────────────────

        public async Task<UiPreferences.UiPreferencesSettings> GetUiPreferencesAsync(Guid accountId)
        {
            var doc = await GetOrCreateAsync(accountId);
            return doc.UiPreferences;
        }

        public Task<int> UpdateUiPreferencesAsync(Guid accountId, UiPreferences.UiPreferencesSettings settings, int expectedVersion)
            => UpdateSliceAsync(accountId, x => x.UiPreferences, settings, expectedVersion);

        // ── Shared helper ──────────────────────────────

        private async Task<int> UpdateSliceAsync<TField>(
            Guid accountId,
            Expression<Func<AccountPreferencesDocument, TField>> field,
            TField value,
            int expectedVersion)
        {
            var update = Builders<AccountPreferencesDocument>.Update
                .Set(field, value)
                .Inc(x => x.Version, 1);

            var result = await _collection.UpdateOneAsync(
                x => x.AccountId == accountId && x.Version == expectedVersion,
                update);

            if (result.MatchedCount == 0)
                throw new VersionConflictException();

            return expectedVersion + 1;
        }
    }
}
