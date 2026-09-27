namespace PickMeUp.Api.Account.Authentication;

// ── Register Request ──────────────────────────────────────
// DTO submitted by the client to create an account with email + password.

public sealed class RegisterRequest
{
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
}
