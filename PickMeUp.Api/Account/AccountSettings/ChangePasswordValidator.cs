using FluentValidation;

namespace PickMeUp.Api.Account.AccountSettings;

// ── Change Password Validator ─────────────────────────────
// Validates the password-change request before it reaches the controller.
// Enforces minimum length and non-empty fields; deeper policy checks
// (uppercase, digit, symbol, length ≥ 12) are enforced by ASP.NET Identity
// and the domain Password value object.

public sealed class ChangePasswordValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .WithMessage("Current password is required.");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("New password is required.")
            .MinimumLength(8)
            .WithMessage("New password must be at least 8 characters long.")
            .NotEqual(x => x.CurrentPassword)
            .WithMessage("New password must differ from the current password.");
    }
}
