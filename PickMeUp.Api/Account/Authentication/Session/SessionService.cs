using Microsoft.Extensions.Caching.Distributed;

namespace PickMeUp.Api.Account.Authentication.Session
{
    // Manages server-side sessions in Redis.

    public sealed class SessionService(IDistributedCache cache, SessionConfig config)
    {
        private readonly IDistributedCache _cache = cache;
        private readonly SessionConfig _config = config;

        // 1. CreateSession() — stores accountId keyed by sessionId.
        // 2. ValidateSession() — checks if a session is still active.
        // 3. GetAccountIdFromSession() — retrieves the account for token generation.
        // 4. RevokeSession() — removes the session (logout).

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
