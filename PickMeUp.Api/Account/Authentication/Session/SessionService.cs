using Microsoft.Extensions.Caching.Distributed;

namespace PickMeUp.Api.Account.Authentication.Session
{
    /// <summary>
    /// Manages server-side sessions in Redis.
    /// Each session maps a sessionId to an accountId with a configurable TTL.
    /// </summary>
    public sealed class SessionService(IDistributedCache cache, SessionConfig config)
    {
        private readonly IDistributedCache _cache = cache;
        private readonly SessionConfig _config = config;

        public async Task CreateSession(Guid accountId, string sessionId)
        {
            await _cache.SetStringAsync(
                $"session:{sessionId}",
                accountId.ToString(),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_config.ExpireInHours)
                }
            );
        }

        public async Task<bool> ValidateSession(string sessionId)
        {
            string? accountId = await _cache.GetStringAsync($"session:{sessionId}");
            return accountId is not null;
        }

        public async Task<string?> GetAccountIdFromSession(string sessionId)
        {
            return await _cache.GetStringAsync($"session:{sessionId}");
        }

        public async Task RevokeSession(string sessionId)
        {
            await _cache.RemoveAsync($"session:{sessionId}");
        }
    }
}
