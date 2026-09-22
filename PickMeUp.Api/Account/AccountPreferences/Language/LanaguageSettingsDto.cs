namespace PickMeUp.Api.Account.AccountPreferences.Language
{
    public sealed class LanguageSettingsDto
    {
        public string PreferredLanguage { get; init; } = "en";

        public int Version { get; init; }

        public LanguageSettings ToSettings() => new()
        {
            PreferredLanguage = PreferredLanguage
        };
    }
}
