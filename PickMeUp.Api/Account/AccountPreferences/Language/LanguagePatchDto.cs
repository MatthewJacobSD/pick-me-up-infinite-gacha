namespace PickMeUp.Api.Account.AccountPreferences.Language;

public sealed record LanguagePatchDto
{
    public string? PreferredLanguage { get; init; }
    public int Version { get; init; }

    public LanguageSettings ApplyTo(LanguageSettings current) => current with
    {
        PreferredLanguage = PreferredLanguage ?? current.PreferredLanguage
    };
}
