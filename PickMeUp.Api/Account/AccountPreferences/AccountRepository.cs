using System.Linq.Expressions;
using MongoDB.Driver;
using PickMeUp.Api.Account.AccountPreferences.Accessibility;
using PickMeUp.Api.Account.AccountPreferences.Audio;
using PickMeUp.Api.Account.AccountPreferences.Gameplay;
using PickMeUp.Api.Account.AccountPreferences.Language;
using PickMeUp.Api.Account.AccountPreferences.Notifications;
using PickMeUp.Api.Account.AccountPreferences.SocialPreferences;
using PickMeUp.Api.Account.AccountPreferences.UiPreferences;
using PickMeUp.Api.Common.Errors;

namespace PickMeUp.Api.Account.AccountPreferences;

public sealed class AccountPreferencesRepository(IMongoDatabase db) : IAccountPreferencesRepository
{
    private readonly IMongoCollection<AccountDocument> _collection =
        db.GetCollection<AccountDocument>("account_preferences");

    public async Task<AccountDocument> GetOrCreateAsync(Guid accountId)
    {
        var update = Builders<AccountDocument>.Update
            .SetOnInsert(x => x.AccountId, accountId)
            .SetOnInsert(x => x.UserId, accountId.ToString())
            .SetOnInsert(x => x.Gameplay, new GameplaySettings())
            .SetOnInsert(x => x.Accessibility, new AccessibilitySettings())
            .SetOnInsert(x => x.Language, new LanguageSettings())
            .SetOnInsert(x => x.Notifications, new NotificationSettings())
            .SetOnInsert(x => x.SocialPreferences, new SocialSettings())
            .SetOnInsert(x => x.Audio, new AudioSettings())
            .SetOnInsert(x => x.UiPreferences, new UiSettings())
            .SetOnInsert(x => x.Version, 1);

        await _collection.UpdateOneAsync(
            x => x.AccountId == accountId,
            update,
            new UpdateOptions { IsUpsert = true });

        return (await _collection.Find(x => x.AccountId == accountId).FirstOrDefaultAsync())!;
    }

    public async Task<AccountDocument?> FindAsync(Guid accountId)
        => await _collection.Find(x => x.AccountId == accountId).FirstOrDefaultAsync();

    // ── Gameplay ──────────────────────────────────────────────────

    public async Task<GameplaySettings> GetGameplaySettingsAsync(Guid accountId)
    {
        var doc = await GetOrCreateAsync(accountId);
        return doc.Gameplay;
    }

    public Task<AccountDocument> ReplaceGameplaySettingsAsync(
        Guid accountId, GameplaySettings settings, int expectedVersion)
        => ReplaceSliceAsync(accountId, x => x.Gameplay, settings, expectedVersion);

    public async Task<AccountDocument> PatchGameplaySettingsAsync(
        Guid accountId, GameplayPatchDto patch, int expectedVersion)
    {
        var doc = await GetOrCreateAsync(accountId);
        if (doc.Version != expectedVersion)
            throw new VersionConflictException();
        var next = patch.ToApply(doc.Gameplay);
        return await ReplaceSliceAsync(accountId, x => x.Gameplay, next, expectedVersion);
    }

    // ── Accessibility ─────────────────────────────────────────────

    public async Task<AccessibilitySettings> GetAccessibilitySettingsAsync(Guid accountId)
    {
        var doc = await GetOrCreateAsync(accountId);
        return doc.Accessibility;
    }

    public Task<AccountDocument> ReplaceAccessibilitySettingsAsync(
        Guid accountId, AccessibilitySettings settings, int expectedVersion)
        => ReplaceSliceAsync(accountId, x => x.Accessibility, settings, expectedVersion);

    public async Task<AccountDocument> PatchAccessibilitySettingsAsync(
        Guid accountId, AccessibilityPatchDto patch, int expectedVersion)
    {
        var doc = await GetOrCreateAsync(accountId);
        if (doc.Version != expectedVersion)
            throw new VersionConflictException();
        var next = patch.ApplyTo(doc.Accessibility);
        return await ReplaceSliceAsync(accountId, x => x.Accessibility, next, expectedVersion);
    }

    // ── Language ──────────────────────────────────────────────────

    public async Task<LanguageSettings> GetLanguageSettingsAsync(Guid accountId)
    {
        var doc = await GetOrCreateAsync(accountId);
        return doc.Language;
    }

    public Task<AccountDocument> ReplaceLanguageSettingsAsync(
        Guid accountId, LanguageSettings settings, int expectedVersion)
        => ReplaceSliceAsync(accountId, x => x.Language, settings, expectedVersion);

    public async Task<AccountDocument> PatchLanguageSettingsAsync(
        Guid accountId, LanguagePatchDto patch, int expectedVersion)
    {
        var doc = await GetOrCreateAsync(accountId);
        if (doc.Version != expectedVersion)
            throw new VersionConflictException();
        var next = patch.ApplyTo(doc.Language);
        return await ReplaceSliceAsync(accountId, x => x.Language, next, expectedVersion);
    }

    // ── Notification ──────────────────────────────────────────────

    public async Task<NotificationSettings> GetNotificationSettingsAsync(Guid accountId)
    {
        var doc = await GetOrCreateAsync(accountId);
        return doc.Notifications;
    }

    public Task<AccountDocument> ReplaceNotificationSettingsAsync(
        Guid accountId, NotificationSettings settings, int expectedVersion)
        => ReplaceSliceAsync(accountId, x => x.Notifications, settings, expectedVersion);

    public async Task<AccountDocument> PatchNotificationSettingsAsync(
        Guid accountId, NotificationPatchDto patch, int expectedVersion)
    {
        var doc = await GetOrCreateAsync(accountId);
        if (doc.Version != expectedVersion)
            throw new VersionConflictException();
        var next = patch.ApplyTo(doc.Notifications);
        return await ReplaceSliceAsync(accountId, x => x.Notifications, next, expectedVersion);
    }

    // ── Social Preferences ────────────────────────────────────────

    public async Task<SocialSettings> GetSocialPreferencesAsync(Guid accountId)
    {
        var doc = await GetOrCreateAsync(accountId);
        return doc.SocialPreferences;
    }

    public Task<AccountDocument> ReplaceSocialPreferencesSettingsAsync(
        Guid accountId, SocialSettings settings, int expectedVersion)
        => ReplaceSliceAsync(accountId, x => x.SocialPreferences, settings, expectedVersion);

    public async Task<AccountDocument> PatchSocialPreferencesSettingsAsync(
        Guid accountId, SocialPatchDto patch, int expectedVersion)
    {
        var doc = await GetOrCreateAsync(accountId);
        if (doc.Version != expectedVersion)
            throw new VersionConflictException();
        var next = patch.ApplyTo(doc.SocialPreferences);
        return await ReplaceSliceAsync(accountId, x => x.SocialPreferences, next, expectedVersion);
    }

    // ── Audio ─────────────────────────────────────────────────────

    public async Task<AudioSettings> GetAudioPreferencesAsync(Guid accountId)
    {
        var doc = await GetOrCreateAsync(accountId);
        return doc.Audio;
    }

    public Task<AccountDocument> ReplaceAudioPreferencesSettingsAsync(
        Guid accountId, AudioSettings settings, int expectedVersion)
        => ReplaceSliceAsync(accountId, x => x.Audio, settings, expectedVersion);

    public async Task<AccountDocument> PatchAudioPreferencesSettingsAsync(
        Guid accountId, AudioPatchDto patch, int expectedVersion)
    {
        var doc = await GetOrCreateAsync(accountId);
        if (doc.Version != expectedVersion)
            throw new VersionConflictException();
        var next = patch.ApplyTo(doc.Audio);
        return await ReplaceSliceAsync(accountId, x => x.Audio, next, expectedVersion);
    }

    // ── UI ────────────────────────────────────────────────────────

    public async Task<UiSettings> GetUiPreferencesAsync(Guid accountId)
    {
        var doc = await GetOrCreateAsync(accountId);
        return doc.UiPreferences;
    }

    public Task<AccountDocument> ReplaceUiPreferencesSettingsAsync(
        Guid accountId, UiSettings settings, int expectedVersion)
        => ReplaceSliceAsync(accountId, x => x.UiPreferences, settings, expectedVersion);

    public async Task<AccountDocument> PatchUiPreferencesSettingsAsync(
        Guid accountId, UiPatchDto patch, int expectedVersion)
    {
        var doc = await GetOrCreateAsync(accountId);
        if (doc.Version != expectedVersion)
            throw new VersionConflictException();
        var next = patch.ApplyTo(doc.UiPreferences);
        return await ReplaceSliceAsync(accountId, x => x.UiPreferences, next, expectedVersion);
    }

    // ── Shared Helper ─────────────────────────────────────────────

    private async Task<AccountDocument> ReplaceSliceAsync<TField>(
        Guid accountId,
        Expression<Func<AccountDocument, TField>> field,
        TField value,
        int expectedVersion)
    {
        var update = Builders<AccountDocument>.Update
            .Set(field, value)
            .Inc(x => x.Version, 1);

        var result = await _collection.FindOneAndUpdateAsync(
            x => x.AccountId == accountId && x.Version == expectedVersion,
            update, new FindOneAndUpdateOptions<AccountDocument>
            {
                ReturnDocument = ReturnDocument.After
            }) ?? throw new VersionConflictException();
        return result;
    }
}
