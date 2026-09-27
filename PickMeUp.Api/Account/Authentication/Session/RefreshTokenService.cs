using Microsoft.Extensions.Caching.Distributed;

namespace PickMeUp.Api.Account.Authentication.Session
{
    /// <summary>
    /// Refresh token rotation service.
    /// Invalidates the old token (one-time use), generates a new access + refresh pair,
    /// and stores the new refresh token in Redis.
    /// </summary>
    public sealed class RefreshTokenService(
        IDistributedCache cache,
        TokenGeneratorService tokenGenerator,
        RefreshTokenConfig config)
    {
        private readonly IDistributedCache _cache = cache;
        private readonly TokenGeneratorService _tokenGenerator = tokenGenerator;
        private readonly RefreshTokenConfig _config = config;

        public async Task<(string accessToken, string refreshToken)> RotateAsync(string oldRefreshToken)
        {
            string? accountId = await _cache.GetStringAsync($"refresh:{oldRefreshToken}");
            if (accountId is null)
                throw new UnauthorizedAccessException("Invalid refresh token");

            // Invalidate old token immediately — single-use.
            await _cache.RemoveAsync($"refresh:{oldRefreshToken}");

            var newAccessToken = _tokenGenerator.GenerateAccessToken(Guid.Parse(accountId));
            var newRefreshToken = _tokenGenerator.GenerateRefreshToken(Guid.Parse(accountId));

            await _cache.SetStringAsync(
                $"refresh:{newRefreshToken}",
                accountId,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(_config.ExpireInDays)
                }
            );

            return (newAccessToken, newRefreshToken);
        }
    }
}
