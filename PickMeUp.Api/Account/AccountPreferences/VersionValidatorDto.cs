using FluentValidation;

namespace PickMeUp.Api.Account.AccountPreferences
{
    /// <summary>
    /// FluentValidation rules for <see cref="VersionDto"/> — version must be non-negative.
    /// </summary>
    public sealed class VersionDtoValidator : AbstractValidator<VersionDto>
    {
        public VersionDtoValidator()
        {
            RuleFor(x => x.Version).GreaterThanOrEqualTo(0);
        }
    }
}
