namespace PickMeUp.Api.Account.AccountPreferences.Social
{
    // ── Block Command ──────────────────────────────────
    // Command object for block/unblock operations.

    public sealed class BlockCommand
    {
        public string TargetUserId { get; private init; }
        public BlockAction Action { get; private init; }

        private BlockCommand(string targetUserId, BlockAction action)
        {
            TargetUserId = targetUserId;
            Action = action;
        }

        public static BlockCommand Create(string targetUserId, BlockAction action)
        {
            if (string.IsNullOrWhiteSpace(targetUserId))
                throw new ArgumentException("Cannot locate target user");

            return new BlockCommand(targetUserId, action);
        }
    }

    public enum BlockAction
    {
        BlockUser = 0,
        UnblockUser = 1
    }
}
