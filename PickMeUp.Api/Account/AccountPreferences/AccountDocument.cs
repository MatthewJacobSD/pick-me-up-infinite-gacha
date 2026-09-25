using MongoDB.Bson.Serialization.Attributes;
using PickMeUp.Api.Account.AccountPreferences.Accessibility;
using PickMeUp.Api.Account.AccountPreferences.Audio;
using PickMeUp.Api.Account.AccountPreferences.Gameplay;
using PickMeUp.Api.Account.AccountPreferences.Language;
using PickMeUp.Api.Account.AccountPreferences.Notifications;
using PickMeUp.Api.Account.AccountPreferences.SocialPreferences;
using PickMeUp.Api.Account.AccountPreferences.UiPreferences;

namespace PickMeUp.Api.Account.AccountPreferences
{
    public sealed class AccountDocument
    {
        [BsonId]
        public Guid AccountId { get; init; }
        public string UserId { get; init; } = string.Empty;

        public Gameplay.GameplaySettings Gameplay { get; init; } = GameplaySettings.Default;
        public AccessibilitySettings Accessibility { get; init; } = AccessibilitySettings.Default;
        public LanguageSettings Language { get; init; } = LanguageSettings.Default;
        public NotificationSettings Notifications { get; init; } = NotificationSettings.Default;
        public SocialSettings SocialPreferences { get; init; } = SocialSettings.Default;
        public AudioSettings Audio { get; init; } = AudioSettings.Default;
        public UiSettings UiPreferences { get; init; } = UiSettings.Default;

        public int Version { get; init; } = 1;
    }
}
