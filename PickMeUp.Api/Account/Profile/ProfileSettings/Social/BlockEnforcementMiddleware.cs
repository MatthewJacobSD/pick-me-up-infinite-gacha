using Microsoft.AspNetCore.Http;

namespace PickMeUp.Api.Account.Profile.ProfileSettings.Social
{
    // ── Block Enforcement Middleware ───────────────────
    // ASP.NET middleware that blocks social actions between mutually blocked users.
    // Place in the pipeline before authorization for social endpoints.

    public sealed class BlockEnforcementMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ISocialRepository _repo;

        public BlockEnforcementMiddleware(RequestDelegate next, ISocialRepository repo)
        {
            _next = next;
            _repo = repo;
        }

        // 1. Extract the current user from the authenticated context.
        // 2. Extract the target user from the route.
        // 3. If either has blocked the other, return 403 Forbidden.
        // 4. Otherwise, pass through to the next middleware.
        public async Task InvokeAsync(HttpContext context)
        {
            var userId = context.User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(userId))
            {
                await _next(context);
                return;
            }

            var targetUserId = context.Request.RouteValues["targetUserId"]?.ToString();

            if (string.IsNullOrWhiteSpace(targetUserId))
            {
                await _next(context);
                return;
            }

            if (await _repo.IsBlockedAsync(userId, targetUserId) ||
                await _repo.IsBlockedAsync(targetUserId, userId))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Social interaction blocked.");
                return;
            }

            await _next(context);
        }
    }
}
