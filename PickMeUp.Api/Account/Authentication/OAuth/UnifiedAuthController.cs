using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using PickMeUp.Api.Account.Authentication.Session;

namespace PickMeUp.Api.Account.Authentication.OAuth
{
    // Main OAuth controller bridging the browser OAuth flow with the game client.
    //
    // Flow:
    //   1. GET /api/auth/login/{provider}      → 302 to Google/Facebook
    //   2. GET /api/auth/login-url/{provider}  → 200 JSON { redirectUrl }
    //   3. GET /api/auth/callback/{provider}   → exchange code, create session, redirect with loginCode
    //   4. POST /api/auth/consume-login-code   → exchange loginCode for tokens
    //   5. POST /api/auth/logout               → revoke session

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
        IDistributedCache cache) : ControllerBase
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

        // ── 1. Login (browser redirect) ──────────────────────────

        [HttpGet("login/{provider}")]
        public IActionResult Login(string provider)
        {
            var parsed = OAuthProviderExtensions.FromString(provider);
            if (!parsed.IsSupported())
                return BadRequest("Unsupported provider");

            string redirectUrl = _loginService.BuildRedirectUrl(parsed);
            return Redirect(redirectUrl);
        }

        // ── 1b. Login URL (JSON for programmatic clients) ─────────

        [HttpGet("login-url/{provider}")]
        public IActionResult LoginUrl(string provider)
        {
            var parsed = OAuthProviderExtensions.FromString(provider);
            if (!parsed.IsSupported())
                return BadRequest("Unsupported provider");

            string redirectUrl = _loginService.BuildRedirectUrl(parsed);
            return Ok(new { redirectUrl });
        }

        // ── 2. Callback (provider redirects here) ─────────────────

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

            // Validate state once — this consumes the Redis key.
            if (!_stateValidator.ValidateState(state))
                return Unauthorized(new { error = "Invalid OAuth state" });

            try
            {
                Console.WriteLine($"[OAuth] Callback: provider={parsed}, code={code?.Substring(0, Math.Min(15, code?.Length ?? 0))}...");

                // Handler assumes state is already validated — does NOT call ValidateState again.
                Console.WriteLine("[OAuth] Step 1: Exchanging code for token...");
                var identity = await _callbackHandler.HandleAsync(parsed, code);
                Console.WriteLine($"[OAuth] Step 2: Got identity: {identity.Email} ({identity.Name})");

                Console.WriteLine("[OAuth] Step 3: Creating account...");
                var account = _accountCreation.CreateFromExternal(identity);
                Console.WriteLine($"[OAuth] Step 4: Account created: {account.Id}");

                _accountLinking.LinkOrGetExisting(account.Id, identity);
                Console.WriteLine("[OAuth] Step 5: Account linked");

                // Create session + loginCode.
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

                // In development, return JSON with account info for debugging.
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    return Ok(new
                    {
                        loginCode,
                        sessionId,
                        personalId = account.PersonalId,
                        identity = new { identity.Provider, identity.Email, identity.Name, identity.ExternalId }
                    });
                }

                // In production, redirect to client app.
                var clientUrl = Environment.GetEnvironmentVariable("CLIENT_COMPLETE_URL")
                    ?? "https://yourgame.com/auth/complete";
                return Redirect($"{clientUrl}?loginCode={loginCode}");
            }
            catch (OAuthHttpException ex)
            {
                Console.WriteLine($"[OAuth] ERROR: OAuthHttpException: {ex.StatusCode} - {ex.Message}");
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
                Console.WriteLine($"[OAuth] ERROR: OAuthException: {ex.Message}");
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
                Console.WriteLine($"[OAuth] UNEXPECTED ERROR: {ex.GetType().Name}: {ex.Message}");
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

        // ── 3. Consume loginCode → tokens ─────────────────────────

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

        // ── 4. Logout ─────────────────────────────────────────────

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest req)
        {
            await _sessionService.RevokeSession(req.SessionId);
            return Ok();
        }
    }

    public sealed record LoginCodeRequest(string LoginCode);
    public sealed record LogoutRequest(string SessionId);
}
