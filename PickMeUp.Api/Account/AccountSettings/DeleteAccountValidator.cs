using FluentValidation;

namespace PickMeUp.Api.Account.AccountSettings;

// ── Delete Account Validator ──────────────────────────────
// Validates the account deletion request.

public sealed class DeleteAccountValidator : AbstractValidator<DeleteAccountRequest>
{
    public DeleteAccountValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required for account deletion.");
    }
}
