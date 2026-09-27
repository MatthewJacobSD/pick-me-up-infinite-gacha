namespace PickMeUp.Api.Account.AccountSettings;

// ── Delete Account Request ────────────────────────────────
// DTO submitted by the client to soft-delete their account.
// Password verification is required before deletion.

public sealed class DeleteAccountRequest
{
    public string Password { get; init; } = null!;
}
