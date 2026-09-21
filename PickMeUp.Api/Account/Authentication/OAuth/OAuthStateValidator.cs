using System.Security.Cryptography;
using Microsoft.Extensions.Caching.Distributed;

namespace PickMeUp.Api.Account.Authentication.OAuth
{
    // CSRF protection for the OAuth flow.

    public sealed class OAuthStateValidator(IDistributedCache cache)
    {
        private readonly IDistributedCache _cache = cache;

        // 1. GenerateState() creates a random 256-bit token, stores it in Redis (5 min TTL).
        // 2. The state is sent to the provider as a query parameter.
        // 3. Provider redirects back with the same state.
        // 4. ValidateState() checks Redis, then immediately deletes it (one-time use).

        public string GenerateState()
        {
            byte[] bytes = RandomNumberGenerator.GetBytes(32);
            string state = Convert.ToHexString(bytes);

            _cache.SetString(
                $"oauth_state:{state}",
                "valid",
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                }
            );

            return state;
        }

        public bool ValidateState(string? returnedState)
        {
            if (string.IsNullOrWhiteSpace(returnedState))
                return false;

            string key = $"oauth_state:{returnedState}";
            string? value = _cache.GetString(key);

            if (value is null)
                return false;

            // Invalidate immediately — state is single-use.
            _cache.Remove(key);

            return true;
        }
    }
}
