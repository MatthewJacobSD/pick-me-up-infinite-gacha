namespace PickMeUp.Api.Account.Profile.ProfileSettings
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

    }
}
