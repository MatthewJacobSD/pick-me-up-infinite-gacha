using System.Security.Claims;
using System.Text.Json;

namespace PickMeUp.Api.Social;

// Supplementary check for route ids only. Body targets are enforced by SocialPolicy in the service.
public sealed class BlockEnforcementMiddleware(RequestDelegate next, ISocialRepository repository)
{
    private readonly RequestDelegate _next = next;
    private readonly ISocialRepository _repository = repository;

    public async Task InvokeAsync(HttpContext context)
    {
        var userId = ReadAccountId(context.User);
        var targetUserId = FirstRouteId(context, "targetUserId", "senderId", "receiverId");

        if (userId is null || targetUserId is null)
        {
            await _next(context);
            return;
        }

        if (await _repository.IsBlockedEitherWayAsync(userId, targetUserId))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/problem+json";
            await JsonSerializer.SerializeAsync(context.Response.Body, new
            {
                type = SocialExceptionHandler.TypeBase + "policy-denied",
                title = "Forbidden",
                status = 403,
                detail = "A block between these players prevents this action.",
                code = "social.blocked"
            });
            return;
        }

        await _next(context);
    }

    private static string? ReadAccountId(ClaimsPrincipal user)
    {
        var value = user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? user.FindFirstValue("sub")
            ?? user.FindFirstValue("accountId");
        return Guid.TryParse(value, out var id) && id != Guid.Empty
            ? id.ToString("D")
            : null;
    }

    private static string? FirstRouteId(HttpContext context, params string[] keys)
    {
        foreach (var key in keys)
        {
            var value = context.Request.RouteValues[key]?.ToString();
            if (!string.IsNullOrWhiteSpace(value))
                return value;
        }

        return null;
    }
}
