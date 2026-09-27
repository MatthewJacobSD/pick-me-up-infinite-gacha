using MongoDB.Bson;
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
    /// <summary>
    /// MongoDB document holding all preference categories for a single account.
    /// Optimistic concurrency via <see cref="Version"/>.
    /// Schema migration is tracked by <see cref="SettingsVersion"/>.
    /// </summary>
    public sealed class AccountDocument
    {
        /// <summary>
        /// Current schema version. Bump this when preference structures change
        /// in a backward-incompatible way. The repository checks this on read
        /// and applies any pending migrations before returning the document.
        /// </summary>
        public const int CurrentSettingsVersion = 1;

        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
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

        /// <summary>
        /// Tracks the schema version of the stored preference data.
        /// When lower than <see cref="CurrentSettingsVersion"/>, the repository
        /// applies pending migrations before returning the document to callers.
        /// Documents created at the current version are stamped with
        /// <see cref="CurrentSettingsVersion"/> automatically.
        /// </summary>
        public int SettingsVersion { get; init; } = 1;
    }
}
