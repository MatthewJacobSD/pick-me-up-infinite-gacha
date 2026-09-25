using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PickMeUp.Api.Common.Authentication;
using PickMeUp.Api.DoNotTouchFolder;

namespace PickMeUp.Api.Account.AccountSettings;

// ── Account Settings Controller ───────────────────────────
// Manages account-level operations (password changes, etc.).
// Not to be confused with AccountPreferences (game settings).

[ApiController]
[Route("account/settings")]
[Authorize]
public sealed class AccountSettingsController : ControllerBase
{
    private readonly ICurrentUser _currentUser;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IValidator<ChangePasswordRequest> _validator;

    public AccountSettingsController(
        ICurrentUser currentUser,
        UserManager<ApplicationUser> userManager,
        IValidator<ChangePasswordRequest> validator)
    {
        _currentUser = currentUser;
        _userManager = userManager;
        _validator = validator;
    }

    [HttpPost("password")]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangePasswordRequest request,
        CancellationToken ct)
    {
        // ── Validate DTO ─────────────────────────────────────
        var validation = await _validator.ValidateAsync(request, ct);
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
}
