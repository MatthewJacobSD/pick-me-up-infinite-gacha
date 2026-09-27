namespace PickMeUp.Api.Account.Authentication;

// ── Recover Request ───────────────────────────────────────
// DTO submitted by the client to recover a soft-deleted account.
// The account must be within the 30-day recovery window.

public sealed class RecoverRequest
{
    public string Email { get; init; } = null!;
}
