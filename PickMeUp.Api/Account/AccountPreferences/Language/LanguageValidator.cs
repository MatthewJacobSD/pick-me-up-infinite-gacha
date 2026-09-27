using FluentValidation;

namespace PickMeUp.Api.Account.AccountPreferences.Language
{
    /// <summary>
    /// FluentValidation rules for <see cref="LanguageSettingsDto"/> — rejects unsupported language codes.
    /// </summary>
    public sealed class LanguageValidator : AbstractValidator<LanguageSettingsDto>
    {
        // Language codes the game currently ships with (must match client-side locale list).
        private static readonly string[] AllowedLanguages =
        [
            "en", "nl", "fr", "de", "it", "es", "pt", "pl", "ru", "ja", "ko", "zh"
        ];

        public LanguageValidator()
        {
            RuleFor(x => x.PreferredLanguage)
                .Must(lang => AllowedLanguages.Contains(lang))
                .WithMessage("Unsupported language code.");
        }
    }
}
