namespace PickMeUp.Api.Account.AccountPreferences.Language;

/// <summary>
/// Partial-update DTO for language preferences (PATCH). Null fields are left unchanged.
/// </summary>
public sealed record LanguagePatchDto
{
    public string? PreferredLanguage { get; init; }
    public int Version { get; init; }

    /// <summary>Merges non-null patch values onto <paramref name="current"/>, returning a new record.</summary>
    public LanguageSettings ApplyTo(LanguageSettings current) => current with
    {
        PreferredLanguage = PreferredLanguage ?? current.PreferredLanguage
    };
}
