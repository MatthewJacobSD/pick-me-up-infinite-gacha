using PickMeUp.Api.Account.AccountPreferences.SocialPreferences;

namespace PickMeUp.Api.Social;

public readonly record struct SocialVisibilitySnapshot(
    SocialVisibility FriendRequests,
    SocialVisibility PartyInvites,
    SocialVisibility Messages,
    SocialVisibility OnlineStatus);

// Narrow read. Social does not take the preferences repository into policy decisions.
public interface ISocialVisibilityQuery
{
    Task<SocialVisibilitySnapshot> GetForAsync(Guid accountId);
}
