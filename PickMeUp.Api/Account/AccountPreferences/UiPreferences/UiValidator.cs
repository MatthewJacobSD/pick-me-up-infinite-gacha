using FluentValidation;

namespace PickMeUp.Api.Account.AccountPreferences.UiPreferences
{
    public sealed class UiValidator : AbstractValidator<UiPreferencesDto>
    {
        public UiValidator()
        {
            RuleFor(x => x.UiScale).InclusiveBetween(0.5f, 2.0f);
            RuleFor(x => x.TextSize).IsInEnum();
            RuleFor(x => x.IconSize).IsInEnum();

            RuleFor(x => x.MinimapPosition).IsInEnum();
            RuleFor(x => x.ChatWindowPosition).IsInEnum();
            RuleFor(x => x.QuestTrackerPosition).IsInEnum();
            RuleFor(x => x.ActionBarLayout).IsInEnum();

            RuleFor(x => x.InventoryLayout).IsInEnum();
            RuleFor(x => x.ChatLayout).IsInEnum();
        }
    }
}
