using FluentValidation;

namespace PickMeUp.Api.Account.Profile;

// ── Profile Validators ────────────────────────────
// Validates profile update DTOs before they reach the repository.

public sealed class UpdateUsernameValidator : AbstractValidator<UpdateUsernameDto>
{
    public UpdateUsernameValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters")
            .MaximumLength(20).WithMessage("Username must be at most 20 characters")
            .Matches(@"^[a-z0-9_-]+$").WithMessage("Username may only contain lowercase letters, digits, underscores, and hyphens");

        RuleFor(x => x.Version)
            .GreaterThanOrEqualTo(0).WithMessage("Version must be non-negative");
    }
}

public sealed class UpdateAvatarValidator : AbstractValidator<UpdateAvatarDto>
{
    public UpdateAvatarValidator()
    {
        RuleFor(x => x.AvatarUrlPath)
            .NotEmpty().WithMessage("Avatar URL path is required");

        RuleFor(x => x.Version)
            .GreaterThanOrEqualTo(0).WithMessage("Version must be non-negative");
    }
}
