using FluentValidation;

namespace PickMeUp.Api.Account.Profile.ProfileSettings.Accessibility
{
    // Validates accessibility settings before they reach the repository.
    // Enforces known colorblind modes and numeric ranges.

    public sealed class AccessibilitySettingsValidator : AbstractValidator<AccessibilitySettingsDto>
    {
        public AccessibilitySettingsValidator()
        {
            RuleFor(x => x.ColorblindMode)
                .Must(x => new[] { "None", "Protanopia", "Deuteranopia", "Tritanopia" }.Contains(x));

            RuleFor(x => x.SubtitleSize).InclusiveBetween(10, 40);
            RuleFor(x => x.SubtitleOpacity).InclusiveBetween(0f, 1f);
            RuleFor(x => x.TextSize).InclusiveBetween(10, 40);
        }
    }
}
