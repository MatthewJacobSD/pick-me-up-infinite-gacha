using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using PickMeUp.Api.Account.Authentication.Session;

namespace PickMeUp.Api.Account.Authentication.OAuth
{
    // Main OAuth controller. Bridges the browser OAuth flow with the Unity client.
    //
    // Flow:
    //   1. Unity calls GET /api/auth/login/{provider} → gets redirect URL.
    //   2. Browser follows redirect, provider authenticates, redirects to callback.
    //   3. GET /api/auth/callback/{provider} → validates state, exchanges code,
    //      creates account, generates a short-lived loginCode.
    //   4. Unity polls POST /api/auth/consume-login-code → exchanges loginCode
    //      for accessToken + refreshToken.
    //   5. POST /api/auth/logout → revokes the session.

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

        // Step 1 — Unity calls this to get the provider's redirect URL.
        [HttpGet("login/{provider}")]
        public IActionResult Login(string provider)
        {
            var parsed = OAuthProviderExtensions.FromString(provider);
            if (!parsed.IsSupported())
                return BadRequest("Unsupported provider");

            string redirectUrl = _loginService.BuildRedirectUrl(parsed);
            return Ok(new { redirectUrl });
        }

        // Step 2 — Provider redirects here after user authenticates.
        // Creates account if needed, generates loginCode for Unity.
        [HttpGet("callback/{provider}")]
        public async Task<IActionResult> Callback(
            string provider,
            [FromQuery] string code,
            [FromQuery] string state)
        {
            var parsed = OAuthProviderExtensions.FromString(provider);
            if (!parsed.IsSupported())
                return BadRequest("Unsupported provider");

            if (!_stateValidator.ValidateState(state))
                return Unauthorized("Invalid OAuth state");

            var identity = await _callbackHandler.HandleAsync(parsed, code, state);

            var account = _accountCreation.CreateFromExternal(identity);
            _accountLinking.LinkOrGetExisting(account.Id, identity);

            // Create session and a short-lived loginCode for Unity to consume.
            string sessionId = Guid.NewGuid().ToString("N");
            await _sessionService.CreateSession(account.Id, sessionId);

            string loginCode = Guid.NewGuid().ToString("N");
            await _cache.SetStringAsync(
                $"login_code:{loginCode}",
                sessionId,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
                }
            );

            // Redirect browser to a page Unity can detect.
            return Redirect($"https://yourgame.com/auth/complete?loginCode={loginCode}");
        }

        // Step 3 — Unity polls this with the loginCode to get JWT tokens.
        [HttpPost("consume-login-code")]
        public async Task<IActionResult> ConsumeLoginCode([FromBody] LoginCodeRequest req)
        {
            string? sessionId = await _cache.GetStringAsync($"login_code:{req.LoginCode}");
            if (sessionId is null)
                return Unauthorized();

            await _cache.RemoveAsync($"login_code:{req.LoginCode}");

            string? accountId = await _sessionService.GetAccountIdFromSession(sessionId);
            if (accountId is null)
                return Unauthorized();

            Guid accountGuid = Guid.Parse(accountId);

            string accessToken = _tokenGenerator.GenerateAccessToken(accountGuid);
            string refreshToken = _tokenGenerator.GenerateRefreshToken(accountGuid);

            // Store refresh token in Redis.
            await _cache.SetStringAsync(
                $"refresh:{refreshToken}",
                accountId,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(_jwt.RefreshToken.ExpireInDays)
                }
            );

            return Ok(new
            {
                sessionId,
                accessToken,
                refreshToken
            });
        }

        // Step 4 — Unity calls this on logout to revoke the session.
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
