using Microsoft.AspNetCore.Http;

namespace PickMeUp.Api.Account.Profile.ProfileSettings.Social
{
    // ASP.NET middleware that blocks social actions between mutually blocked users.
    //
    // Extracts the current user and target user from the route.
    // If either has blocked the other, returns 403 Forbidden.
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

        public async Task InvokeAsync(HttpContext context)
        {
            var userId = context.User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(userId))
            {
                await _next(context);
                return;
            }

            // Extract target user from route
            var targetUserId = context.Request.RouteValues["targetUserId"]?.ToString();

            if (string.IsNullOrWhiteSpace(targetUserId))
            {
                await _next(context);
                return;
            }

            // Bidirectional block check
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
