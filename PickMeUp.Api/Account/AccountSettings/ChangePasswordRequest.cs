namespace PickMeUp.Api.Account.AccountSettings;

// ── Change Password Request ───────────────────────────────
// DTO submitted by the client to change account password.
// CurrentPassword is verified before the new password is applied.

public sealed class ChangePasswordRequest
{
    public string CurrentPassword { get; init; } = null!;
    public string NewPassword { get; init; } = null!;
}
