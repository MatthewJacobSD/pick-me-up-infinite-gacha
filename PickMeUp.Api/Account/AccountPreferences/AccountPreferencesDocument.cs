using MongoDB.Bson.Serialization.Attributes;
using PickMeUp.Api.Account.AccountPreferences.SocialPreferences;

namespace PickMeUp.Api.Account.AccountPreferences
{
    public sealed class AccountPreferencesDocument
    {
        [BsonId]
        public Guid AccountId { get; init; }
        public string UserId { get; init; } = string.Empty;

        public Gameplay.GameplaySettings Gameplay { get; init; } = new();
        public Accessibility.AccessibilitySettings Accessibility { get; init; } = new();
        public Language.LanguageSettings Language { get; init; } = new();
        public Notifications.NotificationSettings Notifications { get; init; } = new();
        public SocialPreferencesSettings SocialPreferences { get; init; } = new();
        public Audio.AudioPreferencesSettings Audio { get; init; } = new();
        public UiPreferences.UiPreferencesSettings UiPreferences { get; init; } = new();

        public int Version { get; init; } = 1;
    }
}
