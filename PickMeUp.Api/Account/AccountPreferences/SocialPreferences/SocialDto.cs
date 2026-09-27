namespace PickMeUp.Api.Account.AccountPreferences.SocialPreferences
{
    /// <summary>
    /// Full-replace DTO for social visibility preferences (PUT). Each channel uses a <see cref="SocialVisibility"/> enum.
    /// </summary>
    public sealed class SocialDto
    {
        public SocialVisibility FriendRequests { get; init; }
        public SocialVisibility Messages { get; init; }
        public SocialVisibility PartyInvites { get; init; }
        public SocialVisibility OnlineStatus { get; init; }

        public int Version { get; init; }

        /// <summary>Maps this DTO to the domain <see cref="SocialSettings"/> record.</summary>
        public SocialSettings ToSettings() => new()
        {
            FriendRequests = FriendRequests,
            Messages = Messages,
            PartyInvites = PartyInvites,
            OnlineStatus = OnlineStatus
        };
    }
}
