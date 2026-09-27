using System.Security.Claims;

namespace PickMeUp.Api.Common.Authentication;

/// <summary>
/// Resolves the current user from HttpContext claims. Supports multiple claim layouts
/// (NameIdentifier, sub, accountId) for compatibility with different auth providers.
/// </summary>
public sealed class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public Guid AccountId
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user is null || !user.Identity?.IsAuthenticated == true)
                return Guid.Empty;

            var value = user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? user.FindFirstValue("sub")
                ?? user.FindFirstValue("accountId");

            return Guid.TryParse(value, out var id) ? id : Guid.Empty;
        }
    }

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true
        && AccountId != Guid.Empty;
}
