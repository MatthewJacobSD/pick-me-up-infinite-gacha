namespace PickMeUp.Api.Account.Profile.ProfileSettings.Social.Social
{
    // ── Party Command ──────────────────────────────────
    // Command object for party invite operations.

    public class PartyCommand
    {
        public string TargetUserId { get; private init; } = string.Empty;
        public PartyAction Action { get; private init; }

        private PartyCommand(string targetUserId, PartyAction action)
        {
            TargetUserId = targetUserId;
            Action = action;
        }

        public static PartyCommand Create(string targetUserId, PartyAction action)
        {
            if (string.IsNullOrWhiteSpace(targetUserId))
                throw new ArgumentException("Cannot locate target user");

            return new PartyCommand(targetUserId, action);
        }
    }

    public enum PartyAction
    {
        InviteToParty = 0,
        AcceptPartyInvite = 1,
        DeclinePartyInvite = 2
    }
}
