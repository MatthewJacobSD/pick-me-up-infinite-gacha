using FluentValidation;

namespace PickMeUp.Api.Account.AccountPreferences.Accessibility
{
    // ── Accessibility Settings Validator ───────────────
    // Validates accessibility settings before they reach the repository.
    // Enforces known colorblind modes and numeric ranges.

    public sealed class AccessibilitySettingsValidator : AbstractValidator<AccessibilitySettingsDto>
    {
        public AccessibilitySettingsValidator()
        {
            RuleFor(x => x.ColorBlindMode).IsInEnum();

            RuleFor(x => x.SubtitleSize).InclusiveBetween(
                AccessibilityLimits.MinTextSize, AccessibilityLimits.MaxTextSize);
            RuleFor(x => x.SubtitleOpacity).InclusiveBetween(
                AccessibilityLimits.MinOpacity, AccessibilityLimits.MaxOpaity);
            RuleFor(x => x.TextSize).InclusiveBetween(
                AccessibilityLimits.MinTextSize, AccessibilityLimits.MaxTextSize);
        }

        public sealed class AccessibilitySettingsPatchValidator : AbstractValidator<AccessibilitySettingsPatchDto>
        {
            public AccessibilitySettingsPatchValidator()
            {
                RuleFor(x => x.ColorBlindMode).IsInEnum().When(x => x.ColorBlindMode.HasValue);
                RuleFor(x => x.SubtitleSize).InclusiveBetween(AccessibilityLimits.MinTextSize, AccessibilityLimits.MaxTextSize).When(x => x.SubtitleSize.HasValue);
                RuleFor(x => x.SubtitleOpacity).InclusiveBetween(AccessibilityLimits.MinOpacity, AccessibilityLimits.MaxOpacity).When(x => x.SubtitleOpacity.HasValue);
                RuleFor(x => x.TextSize).InclusiveBetween(AccessibilityLimits.MinTextSize, AccessibilityLimits.MaxTextSize).When(x => x.TextSize.HasValue);
                RuleFor(x => x.Version).GreaterThanOrEqualTo(0);
            }
        }

        public sealed class VersionDtoValidator : AbstractValidator<VersionDto>
        {
            public VersionDtoValidator()
            {
                RuleFor(x => x.Version).GreaterThanOrEqualTo(0);
            }
        }
    }

    public static class AccessibilityLimits
    {
        public const int MinTextSize = 10;
        public const int MaxTextSize = 40;
        public const float MinOpacity = 0f;
        public const float MaxOpaity = 1f;
    }


}
