using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using PickMeUp.Api.Account.Authentication.Session;

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
    }

    public sealed record LoginCodeRequest(string LoginCode);
    public sealed record LogoutRequest(string SessionId);
}
