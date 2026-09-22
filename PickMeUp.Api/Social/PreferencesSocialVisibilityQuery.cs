using PickMeUp.Api.Account.AccountPreferences;

namespace PickMeUp.Api.Social;

public sealed class PreferencesSocialVisibilityQuery(IAccountPreferencesRepository preferences)
    : ISocialVisibilityQuery
{
    private readonly IAccountPreferencesRepository _preferences = preferences;

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
