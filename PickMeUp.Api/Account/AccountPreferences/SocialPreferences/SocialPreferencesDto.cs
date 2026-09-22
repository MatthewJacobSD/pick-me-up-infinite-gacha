namespace PickMeUp.Api.Account.AccountPreferences.SocialPreferences
{
    public sealed class SocialPreferencesDto
    {
        public SocialVisibility FriendRequests { get; init; }
        public SocialVisibility Messages { get; init; }
        public SocialVisibility PartyInvites { get; init; }
        public SocialVisibility OnlineStatus { get; init; }
    }
}
