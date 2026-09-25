namespace PickMeUp.Api.Account.AccountPreferences.SocialPreferences
{
    public sealed class SocialDto
    {
        public SocialVisibility FriendRequests { get; init; }
        public SocialVisibility Messages { get; init; }
        public SocialVisibility PartyInvites { get; init; }
        public SocialVisibility OnlineStatus { get; init; }

        public int Version { get; init; }

        public SocialSettings ToSettings() => new()
        {
            FriendRequests = FriendRequests,
            Messages = Messages,
            PartyInvites = PartyInvites,
            OnlineStatus = OnlineStatus
        };
    }
}
