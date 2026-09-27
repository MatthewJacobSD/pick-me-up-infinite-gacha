using PickMeUp.Api.Account.AccountPreferences.SocialPreferences;

namespace PickMeUp.Api.Social;

/**--------[SocialVisibility]--------**/

public readonly record struct SocialVisibilitySnapshot(
    SocialVisibility FriendRequests,
    SocialVisibility PartyInvites,
    SocialVisibility Messages,
    SocialVisibility OnlineStatus);

/// <summary>
/// Narrow read for visibility preferences. Social does not take the preferences repository into policy decisions.
/// </summary>
public interface ISocialVisibilityQuery
{
    Task<SocialVisibilitySnapshot> GetForAsync(Guid accountId);
}
