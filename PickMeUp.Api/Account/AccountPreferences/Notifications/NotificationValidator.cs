using FluentValidation;

namespace PickMeUp.Api.Account.AccountPreferences.Notifications
{
    public sealed class NotificationValidator : AbstractValidator<NotificationDto>
    {
        public NotificationValidator()
        {
            // All booleans — no complex validation needed
        }
    }
}
