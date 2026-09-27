using PickMeUp.Api.Account.AccountPreferences;

namespace PickMeUp.Api.Social;

/**--------[Visibility Query]--------**/

/// <summary>
/// Resolves a player's social visibility by reading their account preferences.
/// Implements the <see cref="ISocialVisibilityQuery"/> strategy so the social
/// layer can check what features another player has opted into.
/// </summary>
public sealed class PreferencesSocialVisibilityQuery(IAccountPreferencesRepository preferences)
    : ISocialVisibilityQuery
{
    private readonly IAccountPreferencesRepository _preferences = preferences;

    /// <summary>
    /// Projects the account preferences into a lightweight visibility snapshot
    /// that callers (e.g. friend list, party finder) can inspect without
    /// coupling to the full preferences document.
    /// </summary>
    public async Task<SocialVisibilitySnapshot> GetForAsync(Guid accountId)
    {
        var settings = await _preferences.GetSocialPreferencesAsync(accountId);
        return new SocialVisibilitySnapshot(
            settings.FriendRequests,
            settings.PartyInvites,
            settings.Messages,
            settings.OnlineStatus);
    }
}
