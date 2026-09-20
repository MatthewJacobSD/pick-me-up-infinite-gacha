using Microsoft.Extensions.Caching.Distributed;

namespace PickMeUp.Api.Account.Authentication.Session
{
    // Refresh token rotation service.
    //
    // Flow:
    //   1. Validate old refresh token against Redis.
    //   2. Invalidate the old token (one-time use).
    //   3. Generate new access + refresh tokens.
    //   4. Store the new refresh token in Redis.
    //   5. Return both tokens to the client.

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
            // 1. Validate old refresh token
            string? accountId = await _cache.GetStringAsync($"refresh:{oldRefreshToken}");
            if (accountId is null)
                throw new UnauthorizedAccessException("Invalid refresh token");

            // 2. Invalidate old refresh token
            await _cache.RemoveAsync($"refresh:{oldRefreshToken}");

            // 3. Generate new tokens
            var newAccessToken = _tokenGenerator.GenerateAccessToken(Guid.Parse(accountId));
            var newRefreshToken = _tokenGenerator.GenerateRefreshToken(Guid.Parse(accountId));

            // 4. Store new refresh token
            await _cache.SetStringAsync(
                $"refresh:{newRefreshToken}",
                accountId,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(_config.ExpireInDays)
                }
            );

            // 5. Return both tokens
            return (newAccessToken, newRefreshToken);
        }
    }
}
