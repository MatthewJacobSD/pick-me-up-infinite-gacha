using Microsoft.Extensions.Caching.Distributed;

namespace PickMeUp.Api.Account.Authentication.Session
{
    // Refresh token rotation service.

    public sealed class RefreshTokenService(
        IDistributedCache cache,
        TokenGeneratorService tokenGenerator,
        RefreshTokenConfig config)
    {
        private readonly IDistributedCache _cache = cache;
        private readonly TokenGeneratorService _tokenGenerator = tokenGenerator;
        private readonly RefreshTokenConfig _config = config;

        // 1. Validate old refresh token against Redis.
        // 2. Invalidate the old token (one-time use).
        // 3. Generate new access + refresh tokens.
        // 4. Store the new refresh token in Redis.
        // 5. Return both tokens to the client.

        public async Task<(string accessToken, string refreshToken)> RotateAsync(string oldRefreshToken)
        {
            string? accountId = await _cache.GetStringAsync($"refresh:{oldRefreshToken}");
            if (accountId is null)
                throw new UnauthorizedAccessException("Invalid refresh token");

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
