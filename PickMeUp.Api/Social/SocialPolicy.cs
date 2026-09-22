using PickMeUp.Api.Account.AccountPreferences.SocialPreferences;

namespace PickMeUp.Api.Social;

public enum SocialDenialKind
{
    None = 0,
    Blocked = 1,
    Visibility = 2
}

public readonly record struct SocialDecision(bool Allowed, SocialDenialKind Denial)
{
    public static SocialDecision Allow() => new(true, SocialDenialKind.None);
    public static SocialDecision Deny(SocialDenialKind denial) => new(false, denial);
}

public sealed class SocialPolicy(ISocialRepository repository, ISocialVisibilityQuery visibility)
{
    private readonly ISocialRepository _repository = repository;
    private readonly ISocialVisibilityQuery _visibility = visibility;

    public Task<SocialDecision> CanSendFriendRequest(string actorId, string targetId)
        => Evaluate(actorId, targetId, snapshot => snapshot.FriendRequests);

    public Task<SocialDecision> CanInviteToParty(string actorId, string targetId)
        => Evaluate(actorId, targetId, snapshot => snapshot.PartyInvites);

    public Task<SocialDecision> CanMessage(string actorId, string targetId)
        => Evaluate(actorId, targetId, snapshot => snapshot.Messages);

    public Task<SocialDecision> CanViewOnlineStatus(string actorId, string targetId)
        => Evaluate(actorId, targetId, snapshot => snapshot.OnlineStatus);

    private async Task<SocialDecision> Evaluate(
        string actorId,
        string targetId,
        Func<SocialVisibilitySnapshot, SocialVisibility> select)
    {
        if (await _repository.IsBlockedEitherWayAsync(actorId, targetId))
            return SocialDecision.Deny(SocialDenialKind.Blocked);

        if (!Guid.TryParse(targetId, out var targetAccountId))
            return SocialDecision.Deny(SocialDenialKind.Visibility);

        var snapshot = await _visibility.GetForAsync(targetAccountId);
        var rule = select(snapshot);

        switch (rule)
        {
            case SocialVisibility.Nobody:
                return SocialDecision.Deny(SocialDenialKind.Visibility);
            case SocialVisibility.FriendsOnly:
                if (!await _repository.AreFriendsAsync(actorId, targetId))
                    return SocialDecision.Deny(SocialDenialKind.Visibility);
                return SocialDecision.Allow();
            case SocialVisibility.Everyone:
                return SocialDecision.Allow();
            default:
                return SocialDecision.Deny(SocialDenialKind.Visibility);
        }
    }
}
