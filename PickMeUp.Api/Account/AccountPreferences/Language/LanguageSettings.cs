namespace PickMeUp.Api.Account.AccountPreferences.Language
{
    public sealed record LanguageSettings
    {
        public static LanguageSettings Default { get; } = new();

        // ISO language code: en, nl, fr, de, it, es, etc.
        public string PreferredLanguage { get; init; } = "en";
    }

}
