using PickMeUp.Api.Account.AccountPreferences.SocialPreferences;

namespace PickMeUp.Api.Account.AccountPreferences
{
    // ── Account Preferences Repository ─────────────────
    // Per-user document in MongoDB with per-domain get/update.
    // Each update increments the Version field for optimistic concurrency.

    public interface IAccountPreferencesRepository
    {
        Task<AccountPreferencesDocument> GetAsync(string userId);

        Task<Gameplay.GameplaySettings> GetGameplaySettingsAsync(string userId);
        Task UpdateGameplaySettingsAsync(string userId, Gameplay.GameplaySettings settings);

        Task<Accessibility.AccessibilitySettings> GetAccessibilitySettingsAsync(string userId);
        Task UpdateAccessibilitySettingsAsync(string userId, Accessibility.AccessibilitySettings settings);

        Task<Language.LanguageSettings> GetLanguageSettingsAsync(string userId);
        Task UpdateLanguageSettingsAsync(string userId, Language.LanguageSettings settings);

        Task<Notifications.NotificationSettings> GetNotificationSettingsAsync(string userId);
        Task UpdateNotificationSettingsAsync(string userId, Notifications.NotificationSettings settings);

        Task<SocialPreferencesSettings> GetSocialPreferencesAsync(string userId);
        Task UpdateSocialPreferencesAsync(string userId, SocialPreferencesSettings settings);

        Task<Audio.AudioPreferencesSettings> GetAudioPreferencesAsync(string userId);
        Task UpdateAudioPreferencesAsync(string userId, Audio.AudioPreferencesSettings settings);

        Task<UiPreferences.UiPreferencesSettings> GetUiPreferencesAsync(string userId);
        Task UpdateUiPreferencesAsync(string userId, UiPreferences.UiPreferencesSettings settings);
    }
}
