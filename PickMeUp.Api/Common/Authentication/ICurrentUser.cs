namespace PickMeUp.Api.Common.Authentication;

/// <summary>
/// Provides the current authenticated user's identity within the request scope.
/// </summary>
public interface ICurrentUser
{
    Guid AccountId { get; }
    bool IsAuthenticated { get; }
}
