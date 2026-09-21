using FluentValidation;

namespace PickMeUp.Api.Account.Profile.ProfileSettings.Gameplay
{
    // Validates gameplay settings before they reach the repository.
    // Rejects out-of-range values for action bars, sensitivity, and FOV.

    public sealed class GameplaySettingsValidator : AbstractValidator<GameplaySettingsDto>
    {
        public GameplaySettingsValidator()
        {
            RuleFor(x => x.VisibleActionBars).InclusiveBetween(1, 6);
            RuleFor(x => x.CameraSensitivity).InclusiveBetween(0.1f, 10f);
            RuleFor(x => x.FieldOfView).InclusiveBetween(60f, 120f);
        }
    }
}
