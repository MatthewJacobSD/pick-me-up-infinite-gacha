using FluentValidation;

namespace PickMeUp.Api.Account.AccountPreferences.SocialPreferences
{
    /// <summary>
    /// FluentValidation rules for <see cref="SocialDto"/> — ensures all visibility fields are valid enum values.
    /// </summary>
    public sealed class SocialValidator : AbstractValidator<SocialDto>
    {
        public SocialValidator()
        {
            RuleFor(x => x.FriendRequests).IsInEnum();
            RuleFor(x => x.Messages).IsInEnum();
            RuleFor(x => x.PartyInvites).IsInEnum();
            RuleFor(x => x.OnlineStatus).IsInEnum();
        }
    }
}
