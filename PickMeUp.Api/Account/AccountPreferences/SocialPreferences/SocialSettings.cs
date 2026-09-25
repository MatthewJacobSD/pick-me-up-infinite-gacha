namespace PickMeUp.Api.Account.AccountPreferences.SocialPreferences
{
    public sealed record SocialSettings
    {
        public static SocialSettings Default { get; } = new();

        // Who can send friend requests
        public SocialVisibility FriendRequests { get; init; } = SocialVisibility.Everyone;

        // Who can send messages
        public SocialVisibility Messages { get; init; } = SocialVisibility.FriendsOnly;

        // Who can send party invites
        public SocialVisibility PartyInvites { get; init; } = SocialVisibility.FriendsOnly;

        // Who can see online status
        public SocialVisibility OnlineStatus { get; init; } = SocialVisibility.FriendsOnly;
    }

    public enum SocialVisibility
    {
        Everyone = 0,
        FriendsOnly = 1,
        Nobody = 2
    }
}
