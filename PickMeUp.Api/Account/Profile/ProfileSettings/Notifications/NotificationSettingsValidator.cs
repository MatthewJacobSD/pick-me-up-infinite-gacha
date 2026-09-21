using FluentValidation;

namespace PickMeUp.Api.Account.Profile.ProfileSettings.Notifications
{
    public sealed class NotificationSettingsValidator : AbstractValidator<NotificationSettingsDto>
    {
        public NotificationSettingsValidator()
        {
            // All booleans — no complex validation needed
        }
    }
}
