namespace PickMeUp.Api.Account.Profile.ProfileSettings
{
    // MongoDB document holding all account preferences.
    // One per user. Contains domain-specific settings objects and a version counter.

    public sealed class AccountPreferencesDocument
    {
        public string UserId { get; init; } = string.Empty;

        public Gameplay.GameplaySettings Gameplay { get; init; } = new();
        public Accessibility.AccessibilitySettings Accessibility { get; init; } = new();

        // Increments on every update for optimistic concurrency.
        public int Version { get; init; } = 1;
    }
}
