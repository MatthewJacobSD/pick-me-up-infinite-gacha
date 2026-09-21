namespace PickMeUp.Api.Account.Profile.ProfileSettings
{
    // Repository interface for account preferences.
    // Each user has one AccountPreferencesDocument in MongoDB.
    // Supports per-domain get/update with automatic versioning.

    public interface IAccountPreferencesRepository
    {
        Task<AccountPreferencesDocument> GetAsync(string userId);

        Task<Gameplay.GameplaySettings> GetGameplaySettingsAsync(string userId);
        Task UpdateGameplaySettingsAsync(string userId, Gameplay.GameplaySettings settings);

        Task<Accessibility.AccessibilitySettings> GetAccessibilitySettingsAsync(string userId);
        Task UpdateAccessibilitySettingsAsync(string userId, Accessibility.AccessibilitySettings settings);
    }
}
