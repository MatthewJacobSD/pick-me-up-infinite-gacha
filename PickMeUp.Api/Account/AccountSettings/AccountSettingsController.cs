using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickMeUp.Api.Account.Authentication.OAuth;
using PickMeUp.Api.Common.Authentication;

namespace PickMeUp.Api.Account.AccountSettings;

/// <summary>
/// Manages account-level operations (account deletion).
/// Password change is disabled for OAuth-only accounts.
/// </summary>
[ApiController]
[Route("account/settings")]
[Authorize]
public sealed class AccountSettingsController : ControllerBase
{
    private readonly ICurrentUser _currentUser;
    private readonly IAccountRepository _accountRepository;
    private readonly IValidator<DeleteAccountRequest> _deleteValidator;

    public AccountSettingsController(
        ICurrentUser currentUser,
        IAccountRepository accountRepository,
        IValidator<DeleteAccountRequest> deleteValidator)
    {
        _currentUser = currentUser;
        _accountRepository = accountRepository;
        _deleteValidator = deleteValidator;
    }

    [HttpPost("delete")]
    public async Task<IActionResult> DeleteAccount(
        [FromBody] DeleteAccountRequest request,
        CancellationToken ct)
    {
        var validation = await _deleteValidator.ValidateAsync(request, ct);
        if (!validation.IsValid)
        {
            return BadRequest(new
            {
                errors = validation.Errors
                    .Select(e => new { e.PropertyName, e.ErrorMessage })
            });
        }

        // Soft-delete MongoDB account
        await _accountRepository.SoftDeleteAsync(_currentUser.AccountId);

        return Ok(new
        {
            message = "Account soft-deleted. You have 30 days to recover it.",
            recoveryDeadline = DateTime.UtcNow.AddDays(30)
        });
    }
}
