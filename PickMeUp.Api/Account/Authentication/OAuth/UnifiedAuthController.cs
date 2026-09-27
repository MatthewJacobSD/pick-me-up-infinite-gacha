using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using PickMeUp.Api.Account.Authentication.Session;

namespace PickMeUp.Api.Account.Authentication.OAuth;

/// <summary>
/// Main auth controller: OAuth flow, registration, recovery, session management.
/// Does NOT depend on ASP.NET Identity — works with MongoDB Account entity directly.
/// </summary>
[ApiController]
[Route("api/auth")]
public sealed class AuthController(
    ExternalLoginService loginService,
    OAuthCallbackHandler callbackHandler,
    OAuthStateValidator stateValidator,
    AccountCreationService accountCreation,
    AccountLinkingService accountLinking,
    SessionService sessionService,
    TokenGeneratorService tokenGenerator,
    RefreshTokenService refreshTokenService,
    Jwt jwt,
    IDistributedCache cache,
    IAccountRepository accountRepository,
    IValidator<RegisterRequest> registerValidator,
    IValidator<RecoverRequest> recoverValidator) : ControllerBase
{
    private readonly ExternalLoginService _loginService = loginService;
    private readonly OAuthCallbackHandler _callbackHandler = callbackHandler;
    private readonly OAuthStateValidator _stateValidator = stateValidator;
    private readonly AccountCreationService _accountCreation = accountCreation;
    private readonly AccountLinkingService _accountLinking = accountLinking;
    private readonly SessionService _sessionService = sessionService;
    private readonly TokenGeneratorService _tokenGenerator = tokenGenerator;
    private readonly RefreshTokenService _refreshTokenService = refreshTokenService;
    private readonly Jwt _jwt = jwt;
    private readonly IDistributedCache _cache = cache;
    private readonly IAccountRepository _accountRepository = accountRepository;
    private readonly IValidator<RegisterRequest> _registerValidator = registerValidator;
    private readonly IValidator<RecoverRequest> _recoverValidator = recoverValidator;

    /**--------[Login]--------**/

    [HttpGet("login/{provider}")]
    public IActionResult Login(string provider)
    {
        var parsed = OAuthProviderExtensions.FromString(provider);
        if (!parsed.IsSupported())
            return BadRequest(new { error = "Unsupported provider" });

        string redirectUrl = _loginService.BuildRedirectUrl(parsed);
        return Redirect(redirectUrl);
    }

    [HttpGet("login-url/{provider}")]
    public IActionResult LoginUrl(string provider)
    {
        var parsed = OAuthProviderExtensions.FromString(provider);
        if (!parsed.IsSupported())
            return BadRequest(new { error = "Unsupported provider" });

        string redirectUrl = _loginService.BuildRedirectUrl(parsed);
        return Ok(new { redirectUrl });
    }

    /**--------[Callback]--------**/

    [HttpGet("callback/{provider}")]
    [HttpGet("/signin-{provider}")]
    public async Task<IActionResult> Callback(
        string provider,
        [FromQuery] string code,
        [FromQuery] string state)
    {
        var parsed = OAuthProviderExtensions.FromString(provider);
        if (!parsed.IsSupported())
            return BadRequest(new { error = "Unsupported provider" });

        if (!_stateValidator.ValidateState(state))
            return Unauthorized(new { error = "Invalid OAuth state" });

        try
        {
            var identity = await _callbackHandler.HandleAsync(parsed, code);

            var account = _accountCreation.CreateFromExternal(identity);
            _accountLinking.LinkOrGetExisting(account.Id, identity);

            string sessionId = Guid.NewGuid().ToString("N");
            await _sessionService.CreateSession(account.Id, sessionId);

            string loginCode = Guid.NewGuid().ToString("N");
            await _cache.SetStringAsync(
                $"login_code:{loginCode}",
                sessionId,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
                });

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                return Ok(new
                {
                    loginCode,
                    sessionId,
                    publicCode = account.PublicCode,
                    gameEmail = account.GameEmail,
                    identity = new { identity.Provider, identity.Email, identity.Name, identity.ExternalId }
                });
            }

            var clientUrl = Environment.GetEnvironmentVariable("CLIENT_COMPLETE_URL")
                ?? "https://placeholder_game_url.com/auth/complete";
            return Redirect($"{clientUrl}?loginCode={loginCode}");
        }
        catch (OAuthHttpException ex)
        {
            Console.WriteLine($"[OAuth] ERROR: {ex.StatusCode} - {ex.Message}");
            return StatusCode((int)ex.StatusCode, new
            {
                type = "https://moebius-pick_me_up_infinite_gacha/errors/oauth",
                title = "OAuth Error",
                status = ex.StatusCode,
                detail = ex.Message,
                provider = ex.Provider.ToString()
            });
        }
        catch (OAuthException ex)
        {
            Console.WriteLine($"[OAuth] ERROR: {ex.Message}");
            return Unauthorized(new
            {
                type = "https://moebius-pick_me_up_infinite_gacha/errors/oauth",
                title = "OAuth Error",
                status = 401,
                detail = ex.Message,
                provider = ex.Provider.ToString()
            });
        }
    }

    /**--------[Consume Login Code]--------**/

    [HttpPost("consume-login-code")]
    public async Task<IActionResult> ConsumeLoginCode([FromBody] LoginCodeRequest req)
    {
        string? sessionId = await _cache.GetStringAsync($"login_code:{req.LoginCode}");
        if (sessionId is null)
            return Unauthorized(new { error = "Invalid or expired login code" });

        await _cache.RemoveAsync($"login_code:{req.LoginCode}");

        string? accountId = await _sessionService.GetAccountIdFromSession(sessionId);
        if (accountId is null)
            return Unauthorized(new { error = "Session not found" });

        Guid accountGuid = Guid.Parse(accountId);

        string accessToken = _tokenGenerator.GenerateAccessToken(accountGuid);
        string refreshToken = _tokenGenerator.GenerateRefreshToken(accountGuid);

        await _cache.SetStringAsync(
            $"refresh:{refreshToken}",
            accountId,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(_jwt.RefreshToken.ExpireInDays)
            });

        return Ok(new { sessionId, accessToken, refreshToken });
    }

    /**--------[Register]--------**/

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var validation = await _registerValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(new { errors = validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) });

        // Check if email already exists
        var existing = _accountRepository.FindByEmail(request.Email);
        if (existing is not null)
            return Conflict(new { error = "An account with this email already exists." });

        // Check if soft-deleted
        var deleted = _accountRepository.FindDeletedByEmail(request.Email);
        if (deleted is not null)
        {
            if (deleted.PurgeAt > DateTime.UtcNow)
                return Conflict(new { error = "Account is scheduled for deletion. Use /api/auth/recover to restore." });
        }

        // Create account in MongoDB
        var publicCode = AccountCreationService.GeneratePublicCode();
        var username = $"usr_{Guid.NewGuid().ToString("N")[..16]}";
        var account = _accountCreation.CreateFromRegistration(request.Email, username, publicCode);

        // Create session + loginCode
        string sessionId = Guid.NewGuid().ToString("N");
        await _sessionService.CreateSession(account.Id, sessionId);

        string loginCode = Guid.NewGuid().ToString("N");
        await _cache.SetStringAsync(
            $"login_code:{loginCode}",
            sessionId,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
            });

        return Ok(new
        {
            loginCode,
            publicCode = account.PublicCode,
            gameEmail = account.GameEmail,
            username = account.Username
        });
    }

    /**--------[Recover]--------**/

    [HttpPost("recover")]
    public async Task<IActionResult> Recover([FromBody] RecoverRequest request)
    {
        var validation = await _recoverValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(new { errors = validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) });

        var deleted = _accountRepository.FindDeletedByEmail(request.Email);
        if (deleted is null)
            return NotFound(new { error = "No deleted account found with this email." });

        if (deleted.PurgeAt <= DateTime.UtcNow)
            return StatusCode(410, new { error = "Recovery window expired." });

        var recovered = await _accountRepository.RecoverAsync(request.Email);

        return Ok(new
        {
            publicCode = recovered.PublicCode,
            gameEmail = recovered.GameEmail,
            username = recovered.Username
        });
    }

    /**--------[Logout]--------**/

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest req)
    {
        await _sessionService.RevokeSession(req.SessionId);
        return Ok();
    }
}

public sealed record LoginCodeRequest(string LoginCode);
public sealed record LogoutRequest(string SessionId);
