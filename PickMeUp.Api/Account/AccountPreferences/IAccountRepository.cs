using PickMeUp.Api.Account.AccountPreferences.Accessibility;
using PickMeUp.Api.Account.AccountPreferences.Audio;
using PickMeUp.Api.Account.AccountPreferences.Gameplay;
using PickMeUp.Api.Account.AccountPreferences.Language;
using PickMeUp.Api.Account.AccountPreferences.Notifications;
using PickMeUp.Api.Account.AccountPreferences.SocialPreferences;
using PickMeUp.Api.Account.AccountPreferences.UiPreferences;

namespace PickMeUp.Api.Account.AccountPreferences;

public interface IAccountPreferencesRepository
{
    // General
    Task<AccountDocument> GetOrCreateAsync(Guid accountId);
    Task<AccountDocument?> FindAsync(Guid accountId);

    // Gameplay
    Task<GameplaySettings> GetGameplaySettingsAsync(Guid accountId);
    Task<AccountDocument> ReplaceGameplaySettingsAsync(Guid accountId, GameplaySettings settings, int expectedVersion);
    Task<AccountDocument> PatchGameplaySettingsAsync(Guid accountId, GameplayPatchDto patch, int expectedVersion);

    // Accessibility
    Task<AccessibilitySettings> GetAccessibilitySettingsAsync(Guid accountId);
    Task<AccountDocument> ReplaceAccessibilitySettingsAsync(Guid accountId, AccessibilitySettings settings, int expectedVersion);
    Task<AccountDocument> PatchAccessibilitySettingsAsync(Guid accountId, AccessibilityPatchDto patch, int expectedVersion);

    // Language
    Task<LanguageSettings> GetLanguageSettingsAsync(Guid accountId);
    Task<AccountDocument> ReplaceLanguageSettingsAsync(Guid accountId, LanguageSettings settings, int expectedVersion);
    Task<AccountDocument> PatchLanguageSettingsAsync(Guid accountId, LanguagePatchDto patch, int expectedVersion);

    // Notification
    Task<NotificationSettings> GetNotificationSettingsAsync(Guid accountId);
    Task<AccountDocument> ReplaceNotificationSettingsAsync(Guid accountId, NotificationSettings settings, int expectedVersion);
    Task<AccountDocument> PatchNotificationSettingsAsync(Guid accountId, NotificationPatchDto patch, int expectedVersion);

    // Social Preferences
    Task<SocialSettings> GetSocialPreferencesAsync(Guid accountId);
    Task<AccountDocument> ReplaceSocialPreferencesSettingsAsync(Guid accountId, SocialSettings settings, int expectedVersion);
    Task<AccountDocument> PatchSocialPreferencesSettingsAsync(Guid accountId, SocialPatchDto patch, int expectedVersion);

    // Audio
    Task<AudioSettings> GetAudioPreferencesAsync(Guid accountId);
    Task<AccountDocument> ReplaceAudioPreferencesSettingsAsync(Guid accountId, AudioSettings settings, int expectedVersion);
    Task<AccountDocument> PatchAudioPreferencesSettingsAsync(Guid accountId, AudioPatchDto patch, int expectedVersion);

    // UI
    Task<UiSettings> GetUiPreferencesAsync(Guid accountId);
    Task<AccountDocument> ReplaceUiPreferencesSettingsAsync(Guid accountId, UiSettings settings, int expectedVersion);
    Task<AccountDocument> PatchUiPreferencesSettingsAsync(Guid accountId, UiPatchDto patch, int expectedVersion);
}
