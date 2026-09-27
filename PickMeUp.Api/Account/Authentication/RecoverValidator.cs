using FluentValidation;

namespace PickMeUp.Api.Account.Authentication;

// ── Recover Validator ─────────────────────────────────────
// Validates the account recovery request.

public sealed class RecoverValidator : AbstractValidator<RecoverRequest>
{
    public RecoverValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("A valid email address is required.");
    }
}
