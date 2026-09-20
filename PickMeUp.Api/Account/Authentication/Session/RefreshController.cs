using Microsoft.AspNetCore.Mvc;

namespace PickMeUp.Api.Account.Authentication.Session
{
    // Endpoint for refreshing expired access tokens.
    //
    // Flow:
    //   1. Client sends a valid refresh token.
    //   2. RefreshTokenService rotates it (invalidates old, creates new).
    //   3. Returns new access + refresh tokens.

    [ApiController]
    [Route("api/auth")]
    public sealed class RefreshController(
        RefreshTokenService refreshTokenService) : ControllerBase
    {
        private readonly RefreshTokenService _refreshTokenService = refreshTokenService;

        public sealed record RefreshRequest(string RefreshToken);

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.RefreshToken))
                return BadRequest("Refresh token is required");

            try
            {
                var (accessToken, refreshToken) =
                    await _refreshTokenService.RotateAsync(req.RefreshToken);

                return Ok(new
                {
                    accessToken,
                    refreshToken
                });
            }
            catch
            {
                return Unauthorized("Invalid or expired refresh token");
            }
        }
    }
}
