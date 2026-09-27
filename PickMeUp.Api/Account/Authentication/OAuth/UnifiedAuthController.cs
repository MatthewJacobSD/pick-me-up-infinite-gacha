using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using PickMeUp.Api.Account.Authentication.Session;
using PickMeUp.Api.DoNotTouchFolder;

namespace PickMeUp.Api.Account.Authentication.OAuth
{
    /// <summary>
    /// Main OAuth controller bridging the browser OAuth flow with the game client.
    ///
    /// Flow:
    ///   1. GET  /api/auth/login/{provider}       → 302 to Google/Facebook
    ///   2. GET  /api/auth/login-url/{provider}   → 200 JSON { redirectUrl }
    ///   3. GET  /api/auth/callback/{provider}    → exchange code, create session, redirect with loginCode
    ///   4. POST /api/auth/consume-login-code     → exchange loginCode for tokens
    ///   5. POST /api/auth/logout                 → revoke session
    ///   6. POST /api/auth/register               → email+password registration
    ///   7. POST /api/auth/recover                → recover soft-deleted account
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
        UserManager<ApplicationUser> userManager,
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
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IAccountRepository _accountRepository = accountRepository;
        private readonly IValidator<RegisterRequest> _registerValidator = registerValidator;
        private readonly IValidator<RecoverRequest> _recoverValidator = recoverValidator;

        /**--------[1. Login (Browser Redirect)]--------**/

        [HttpGet("login/{provider}")]
        public IActionResult Login(string provider)
        {
            var parsed = OAuthProviderExtensions.FromString(provider);
            if (!parsed.IsSupported())
                return BadRequest("Unsupported provider");

            string redirectUrl = _loginService.BuildRedirectUrl(parsed);
            return Redirect(redirectUrl);
        }

        /**--------[1b. Login URL (JSON for Programmatic Clients)]--------**/

        [HttpGet("login-url/{provider}")]
        public IActionResult LoginUrl(string provider)
        {
            var parsed = OAuthProviderExtensions.FromString(provider);
            if (!parsed.IsSupported())
                return BadRequest("Unsupported provider");

            string redirectUrl = _loginService.BuildRedirectUrl(parsed);
            return Ok(new { redirectUrl });
        }

        /**--------[2. Callback (Provider Redirects Here)]--------**/

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
                Console.WriteLine($"[OAuth] Callback: provider={parsed}, code={code?.Substring(0, Math.Min(15, code?.Length ?? 0))}...");

                var identity = await _callbackHandler.HandleAsync(parsed, code);
                Console.WriteLine($"[OAuth] Got identity: {identity.Email} ({identity.Name})");

                var account = _accountCreation.CreateFromExternal(identity);
                Console.WriteLine($"[OAuth] Account: {account.Id}");

                _accountLinking.LinkOrGetExisting(account.Id, identity);

                // Create session + one-time loginCode
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

                // Dev: return JSON with full details for debugging.
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    return Ok(new
                    {
                        loginCode,
                        sessionId,
                        publicCode = account.PublicCode,
                        identity = new { identity.Provider, identity.Email, identity.Name, identity.ExternalId }
                    });
                }

                // Prod: redirect to client app with the one-time code.
                var clientUrl = Environment.GetEnvironmentVariable("CLIENT_COMPLETE_URL")
                    ?? "https://yourgame.com/auth/complete";
                return Redirect($"{clientUrl}?loginCode={loginCode}");
            }
            catch (OAuthHttpException ex)
            {
                Console.WriteLine($"[OAuth] OAuthHttpException: {ex.StatusCode} - {ex.Message}");
                return StatusCode((int)ex.StatusCode, new
                {
                    type = "https://pickmeup/errors/oauth",
                    title = "OAuth Error",
                    status = ex.StatusCode,
                    detail = ex.Message,
                    provider = ex.Provider.ToString()
                });
            }
            catch (OAuthException ex)
            {
                Console.WriteLine($"[OAuth] OAuthException: {ex.Message}");
                return Unauthorized(new
                {
                    type = "https://pickmeup/errors/oauth",
                    title = "OAuth Error",
                    status = 401,
                    detail = ex.Message,
                    provider = ex.Provider.ToString()
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[OAuth] Unexpected: {ex.GetType().Name}: {ex.Message}");
                Console.WriteLine($"[OAuth] Stack: {ex.StackTrace}");
                return StatusCode(500, new
                {
                    type = "https://pickmeup/errors/internal",
                    title = "Internal Server Error",
                    status = 500,
                    detail = ex.Message
                });
            }
        }

        /**--------[3. Consume Login Code → Tokens]--------**/

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

        /**--------[4. Logout]--------**/

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest req)
        {
            await _sessionService.RevokeSession(req.SessionId);
            return Ok();
        }

        /**--------[5. Register (Email + Password)]--------**/

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var validation = await _registerValidator.ValidateAsync(request);
            if (!validation.IsValid)
            {
                return BadRequest(new
                {
                    errors = validation.Errors
                        .Select(e => new { e.PropertyName, e.ErrorMessage })
                });
            }

            // Check if email is already taken in Identity
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser is not null)
                return Conflict(new { error = "An account with this email already exists." });

            // Create Identity user
            var validatedEmail = global::PickMeUp.Api.Account.Authentication.Email.Create(request.Email);
            var user = new ApplicationUser(request.Email, validatedEmail);
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errorMessages = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new { errors = errorMessages });
            }

            // Create MongoDB account with game email
            var publicCode = GeneratePublicCode();
            var username = $"usr_{Guid.NewGuid().ToString("N")[..16]}";
            var account = _accountCreation.CreateFromRegistration(request.Email, username, publicCode);

            // Link external account (email-only, no OAuth provider)
            // Skip linking for now — OAuth linking happens via the OAuth flow.

            // Create session + one-time loginCode
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
                message = "Account registered successfully.",
                publicCode = account.PublicCode,
                gameEmail = account.GameEmail,
                loginCode
            });
        }

        /**--------[6. Recover Soft-Deleted Account]--------**/

        [HttpPost("recover")]
        public async Task<IActionResult> Recover([FromBody] RecoverRequest request)
        {
            var validation = await _recoverValidator.ValidateAsync(request);
            if (!validation.IsValid)
            {
                return BadRequest(new
                {
                    errors = validation.Errors
                        .Select(e => new { e.PropertyName, e.ErrorMessage })
                });
            }

            // Look for the account in the deleted collection
            var deletedAccount = _accountRepository.FindDeletedByEmail(request.Email);
            if (deletedAccount is null)
                return NotFound(new { error = "No deleted account found with this email." });

            // Check 30-day recovery window
            if (deletedAccount.PurgeAt is null || deletedAccount.PurgeAt <= DateTime.UtcNow)
                return StatusCode(410, new { error = "Recovery window has expired. The account has been purged." });

            // Recover: move back to active collection
            _accountRepository.RecoverAsync(request.Email);

            return Ok(new
            {
                message = "Account recovered successfully.",
                publicCode = deletedAccount.PublicCode,
                gameEmail = deletedAccount.GameEmail
            });
        }

        /**--------[Helpers]--------**/

        /// <summary>Public code: MOE- + 10 Crockford Base32 chars = 15 chars total.</summary>
        private static string GeneratePublicCode()
        {
            var bytes = System.Security.Cryptography.RandomNumberGenerator.GetBytes(5);
            var code = CrockfordBase32.Encode(bytes);
            return $"MOE-{code}";
        }
    }

    public sealed record LoginCodeRequest(string LoginCode);
    public sealed record LogoutRequest(string SessionId);
}
