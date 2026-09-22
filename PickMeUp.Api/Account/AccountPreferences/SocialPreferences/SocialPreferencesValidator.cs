using FluentValidation;

namespace PickMeUp.Api.Account.AccountPreferences.SocialPreferences
{
    public sealed class SocialPreferencesValidator : AbstractValidator<SocialPreferencesDto>
    {
        public SocialPreferencesValidator()
        {
            RuleFor(x => x.FriendRequests).IsInEnum();
            RuleFor(x => x.Messages).IsInEnum();
            RuleFor(x => x.PartyInvites).IsInEnum();
            RuleFor(x => x.OnlineStatus).IsInEnum();
        }
    }
}
