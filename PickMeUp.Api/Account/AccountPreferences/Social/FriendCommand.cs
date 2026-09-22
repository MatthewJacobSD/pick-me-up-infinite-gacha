namespace PickMeUp.Api.Account.AccountPreferences.Social
{
    // ── Friend Command ─────────────────────────────────
    // Command object for friend operations.

    public sealed class FriendCommand
    {
        public string TargetUserId { get; private init; } = string.Empty;
        public FriendAction Action { get; private init; }

        private FriendCommand(string targetUserId, FriendAction action)
        {
            TargetUserId = targetUserId;
            Action = action;
        }

        public static FriendCommand Create(string targetUserId, FriendAction action)
        {
            if (string.IsNullOrWhiteSpace(targetUserId))
                throw new ArgumentException("Cannot locate target user");

            return new FriendCommand(targetUserId, action);
        }
    }

    public enum FriendAction
    {
        AddFriend = 0,
        RemoveFriend = 1,
        AcceptFriendRequest = 2,
        DeclineFriendRequest = 3
    }
}
