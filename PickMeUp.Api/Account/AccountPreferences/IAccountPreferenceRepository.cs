using PickMeUp.Api.Account.AccountPreferences.SocialPreferences;

namespace PickMeUp.Api.Account.AccountPreferences
{
    public interface IAccountPreferencesRepository
    {
        Task<AccountPreferencesDocument> GetOrCreateAsync(Guid accountId);
        Task<AccountPreferencesDocument?> FindAsync(Guid accountId);

        Task<Gameplay.GameplaySettings> GetGameplaySettingsAsync(Guid accountId);
        Task<int> UpdateGameplaySettingsAsync(Guid accountId, Gameplay.GameplaySettings settings, int expectedVersion);

        Task<Accessibility.AccessibilitySettings> GetAccessibilitySettingsAsync(Guid accountId);
        Task<int> UpdateAccessibilitySettingsAsync(Guid accountId, Accessibility.AccessibilitySettings settings, int expectedVersion);

        Task<Language.LanguageSettings> GetLanguageSettingsAsync(Guid accountId);
        Task<int> UpdateLanguageSettingsAsync(Guid accountId, Language.LanguageSettings settings, int expectedVersion);

        Task<Notifications.NotificationSettings> GetNotificationSettingsAsync(Guid accountId);
        Task<int> UpdateNotificationSettingsAsync(Guid accountId, Notifications.NotificationSettings settings, int expectedVersion);

        Task<SocialPreferencesSettings> GetSocialPreferencesAsync(Guid accountId);
        Task<int> UpdateSocialPreferencesAsync(Guid accountId, SocialPreferencesSettings settings, int expectedVersion);

        Task<Audio.AudioPreferencesSettings> GetAudioPreferencesAsync(Guid accountId);
        Task<int> UpdateAudioPreferencesAsync(Guid accountId, Audio.AudioPreferencesSettings settings, int expectedVersion);

        Task<UiPreferences.UiPreferencesSettings> GetUiPreferencesAsync(Guid accountId);
        Task<int> UpdateUiPreferencesAsync(Guid accountId, UiPreferences.UiPreferencesSettings settings, int expectedVersion);
    }
}
