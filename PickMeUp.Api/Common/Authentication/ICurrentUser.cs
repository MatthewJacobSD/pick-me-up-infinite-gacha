namespace PickMeUp.Api.Common.Authentication;

public interface ICurrentUser
{
    Guid AccountId { get; }
    bool IsAuthenticated { get; }
}
