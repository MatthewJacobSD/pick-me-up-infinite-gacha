using FluentValidation;

namespace PickMeUp.Api.Account.AccountPreferences
{
    public sealed class VersionDtoValidator : AbstractValidator<VersionDto>
    {
        public VersionDtoValidator()
        {
            RuleFor(x => x.Version).GreaterThanOrEqualTo(0);
        }
    }
}
