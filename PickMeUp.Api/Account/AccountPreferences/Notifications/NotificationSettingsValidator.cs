using FluentValidation;

namespace PickMeUp.Api.Account.AccountPreferences.Notifications
{
    public sealed class NotificationSettingsValidator : AbstractValidator<NotificationSettingsDto>
    {
        public NotificationSettingsValidator()
        {
            // All booleans — no complex validation needed
        }
    }
}
