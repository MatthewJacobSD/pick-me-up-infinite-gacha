using Microsoft.AspNetCore.Mvc;

namespace PickMeUp.Api.Account.Authentication.Session
{
    /// <summary>
    /// Endpoint for refreshing expired access tokens via one-time refresh token rotation.
    /// </summary>
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
