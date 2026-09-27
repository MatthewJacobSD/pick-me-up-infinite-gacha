using System.Security.Cryptography;
using Microsoft.Extensions.Caching.Distributed;

namespace PickMeUp.Api.Account.Authentication.OAuth
{
    /// <summary>
    /// CSRF protection for the OAuth flow.
    /// Generates a random 256-bit state token, stores it in Redis (5 min TTL),
    /// and validates it exactly once (deletes on use).
    /// </summary>
    public sealed class OAuthStateValidator(IDistributedCache cache)
    {
        private readonly IDistributedCache _cache = cache;

        /// <summary>
        /// Generates a random state token and persists it in Redis for 5 minutes.
        /// </summary>
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

        /// <summary>
        /// Validates the returned state against Redis, then immediately deletes it.
        /// </summary>
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
