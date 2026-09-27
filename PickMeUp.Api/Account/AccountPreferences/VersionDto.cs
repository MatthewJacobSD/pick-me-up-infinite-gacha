using FluentValidation;

namespace PickMeUp.Api.Account.AccountPreferences
{
    /// <summary>
    /// Lightweight DTO carrying only an optimistic-concurrency version number.
    /// Used by endpoints that need a version check without sending full settings.
    /// </summary>
    public sealed class VersionDto
    {
        public int Version { get; init; }
    }
}
