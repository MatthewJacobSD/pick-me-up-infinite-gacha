using FluentValidation;

namespace PickMeUp.Api.Account.AccountPreferences.Language
{
    public sealed class LanguageValidator : AbstractValidator<LanguageSettingsDto>
    {
        private static readonly string[] AllowedLanguages =
        {
            "en", "nl", "fr", "de", "it", "es", "pt", "pl", "ru", "ja", "ko", "zh"
        };

        public LanguageValidator()
        {
            RuleFor(x => x.PreferredLanguage)
                .Must(lang => AllowedLanguages.Contains(lang))
                .WithMessage("Unsupported language code.");
        }
    }
}
