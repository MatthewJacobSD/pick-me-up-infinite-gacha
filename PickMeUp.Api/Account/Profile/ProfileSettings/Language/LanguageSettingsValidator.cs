using FluentValidation;

namespace PickMeUp.Api.Account.Profile.ProfileSettings.Language
{
    public sealed class LanguageSettingsValidator : AbstractValidator<LanguageSettingsDto>
    {
        private static readonly string[] AllowedLanguages =
        {
            "en", "nl", "fr", "de", "it", "es", "pt", "pl", "ru", "ja", "ko", "zh"
        };

        public LanguageSettingsValidator()
        {
            RuleFor(x => x.PreferredLanguage)
                .Must(lang => AllowedLanguages.Contains(lang))
                .WithMessage("Unsupported language code.");
        }
    }
}
