using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PickMeUp.Api.Account.Authentication.OAuth;
using PickMeUp.Api.Common.Authentication;
using PickMeUp.Api.DoNotTouchFolder;

namespace PickMeUp.Api.Account.AccountSettings;

// ── Account Settings Controller ───────────────────────────
// Manages account-level operations (password changes, account deletion, etc.).
// Not to be confused with AccountPreferences (game settings).

[ApiController]
[Route("account/settings")]
[Authorize]
public sealed class AccountSettingsController : ControllerBase
{
    private readonly ICurrentUser _currentUser;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IValidator<ChangePasswordRequest> _passwordValidator;
    private readonly IValidator<DeleteAccountRequest> _deleteValidator;
    private readonly IAccountRepository _accountRepository;

    public AccountSettingsController(
        ICurrentUser currentUser,
        UserManager<ApplicationUser> userManager,
        IValidator<ChangePasswordRequest> passwordValidator,
        IValidator<DeleteAccountRequest> deleteValidator,
        IAccountRepository accountRepository)
    {
        _currentUser = currentUser;
        _userManager = userManager;
        _passwordValidator = passwordValidator;
        _deleteValidator = deleteValidator;
        _accountRepository = accountRepository;
    }

    [HttpPost("password")]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangePasswordRequest request,
        CancellationToken ct)
    {
        // ── Validate DTO ─────────────────────────────────────
        var validation = await _passwordValidator.ValidateAsync(request, ct);
        if (!validation.IsValid)
        {
            return BadRequest(new
            {
                errors = validation.Errors
                    .Select(e => new { e.PropertyName, e.ErrorMessage })
            });
        }

        // ── Resolve user ─────────────────────────────────────
        var user = await _userManager.FindByIdAsync(_currentUser.AccountId.ToString());
        if (user is null)
            return Unauthorized(new { error = "User not found." });

        // ── Change password via Identity ─────────────────────
        var result = await _userManager.ChangePasswordAsync(
            user, request.CurrentPassword, request.NewPassword);

        if (result.Succeeded)
            return Ok(new { message = "Password changed successfully." });

        // ── Map Identity errors to HTTP responses ────────────
        var errorMessages = result.Errors.Select(e => e.Description).ToList();

        if (result.Errors.Any(e => e.Code == "PasswordMismatch"))
            return Unauthorized(new { error = "Current password is incorrect." });

        return BadRequest(new { errors = errorMessages });
    }

    [HttpPost("delete")]
    public async Task<IActionResult> DeleteAccount(
        [FromBody] DeleteAccountRequest request,
        CancellationToken ct)
    {
        // ── Validate DTO ─────────────────────────────────────
        var validation = await _deleteValidator.ValidateAsync(request, ct);
        if (!validation.IsValid)
        {
            return BadRequest(new
            {
                errors = validation.Errors
                    .Select(e => new { e.PropertyName, e.ErrorMessage })
            });
        }

        // ── Resolve user ─────────────────────────────────────
        var user = await _userManager.FindByIdAsync(_currentUser.AccountId.ToString());
        if (user is null)
            return Unauthorized(new { error = "User not found." });

        // ── Verify password before deletion ──────────────────
        var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!passwordValid)
            return Unauthorized(new { error = "Incorrect password." });

        // ── Soft-delete MongoDB account ──────────────────────
        _accountRepository.SoftDeleteAsync(_currentUser.AccountId);

        // ── Revoke all sessions ──────────────────────────────
        // Sessions are cleared on next auth attempt (token will be invalid).

        return Ok(new
        {
            message = "Account soft-deleted. You have 30 days to recover it.",
            recoveryDeadline = DateTime.UtcNow.AddDays(30)
        });
    }
}
