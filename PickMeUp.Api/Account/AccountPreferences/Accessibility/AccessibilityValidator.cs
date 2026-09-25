using FluentValidation;

namespace PickMeUp.Api.Account.AccountPreferences.Accessibility
{
    // -----------------[Accessibility Validator]-----------------------
    public sealed class AccessibilityValidator : AbstractValidator<AccessibilityDto>
    {
        public AccessibilityValidator()
        {
            RuleFor(x => x.ColorBlindMode).IsInEnum();

            RuleFor(x => x.SubtitleSize).InclusiveBetween(
                AccessibilityLimits.MinTextSize, AccessibilityLimits.MaxTextSize);
            RuleFor(x => x.SubtitleOpacity).InclusiveBetween(
                AccessibilityLimits.MinOpacity, AccessibilityLimits.MaxOpacity);
            RuleFor(x => x.TextSize).InclusiveBetween(
                AccessibilityLimits.MinTextSize, AccessibilityLimits.MaxTextSize);

            RuleFor(x => x.Version).GreaterThanOrEqualTo(0);
        }

        public sealed class AccessibilitySettingsPatchValidator : AbstractValidator<AccessibilityPatchDto>
        {
            public AccessibilitySettingsPatchValidator()
            {
                RuleFor(x => x.ColorBlindMode).IsInEnum().When(x => x.ColorBlindMode.HasValue);

                RuleFor(x => x.SubtitleSize).InclusiveBetween(
                    AccessibilityLimits.MinTextSize, AccessibilityLimits.MaxTextSize).When(x => x.SubtitleSize.HasValue);
                RuleFor(x => x.SubtitleOpacity).InclusiveBetween(
                    AccessibilityLimits.MinOpacity, AccessibilityLimits.MaxOpacity).When(x => x.SubtitleOpacity.HasValue);
                RuleFor(x => x.TextSize).InclusiveBetween(
                    AccessibilityLimits.MinTextSize, AccessibilityLimits.MaxTextSize).When(x => x.TextSize.HasValue);
                
                RuleFor(x => x.Version).GreaterThanOrEqualTo(0);
            }
        }
    }

    public static class AccessibilityLimits
    {
        public const int MinTextSize = 10;
        public const int MaxTextSize = 40;
        public const float MinOpacity = 0f;
        public const float MaxOpacity = 1f;
    }


}
