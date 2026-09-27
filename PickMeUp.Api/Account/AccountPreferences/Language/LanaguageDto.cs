namespace PickMeUp.Api.Account.AccountPreferences.Language
{
    /// <summary>
    /// Full-replace DTO for language preferences (PUT). Defaults to English.
    /// </summary>
    public sealed class LanguageSettingsDto
    {
        public string PreferredLanguage { get; init; } = "en";

        public int Version { get; init; }

        /// <summary>Maps this DTO to the domain <see cref="LanguageSettings"/> record.</summary>
        public LanguageSettings ToSettings() => new()
        {
            PreferredLanguage = PreferredLanguage
        };
    }
}
