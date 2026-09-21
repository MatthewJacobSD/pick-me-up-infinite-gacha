namespace PickMeUp.Api.Account.Profile.ProfileSettings
{
    // ── Account Preferences Document ───────────────────
    // MongoDB document holding all account preferences.
    // One per user. Contains domain-specific settings objects and a version counter.

    public sealed class AccountPreferencesDocument
    {
        public string UserId { get; init; } = string.Empty;

        public Gameplay.GameplaySettings Gameplay { get; init; } = new();
        public Accessibility.AccessibilitySettings Accessibility { get; init; } = new();
        public Language.LanguageSettings Language { get; init; } = new();
        public Notifications.NotificationSettings Notifications { get; init; } = new();


        // Optimistic concurrency counter — increments on every update.
        public int Version { get; init; } = 1;
    }
}
