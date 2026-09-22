using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using PickMeUp.Api.Account.AccountPreferences.SocialPreferences;
using PickMeUp.Api.Social;
using Xunit;

namespace PickMeUp.Api.Tests.Social;

public sealed class SocialBehaviorTests
{
    private readonly Guid _actor = Guid.NewGuid();
    private readonly Guid _target = Guid.NewGuid();
    private readonly InMemorySocialRepository _repository = new();
    private readonly StubVisibility _visibility = new();
    private readonly SocialService _service;

    public SocialBehaviorTests()
    {
        var policy = new SocialPolicy(_repository, _visibility);
        var lifecycle = new FriendRequestLifecycleEngine(_repository);
        _service = new SocialService(_repository, lifecycle, policy);
    }

    [Fact]
    public async Task Policy_BlockedByActor_Denies()
    {
        await _repository.AddBlockAsync(Id(_actor), Id(_target));
        var decision = await new SocialPolicy(_repository, _visibility).CanSendFriendRequest(Id(_actor), Id(_target));
        decision.Allowed.Should().BeFalse();
        decision.Denial.Should().Be(SocialDenialKind.Blocked);
    }

    [Fact]
    public async Task Policy_BlockedByTarget_Denies()
    {
        await _repository.AddBlockAsync(Id(_target), Id(_actor));
        var decision = await new SocialPolicy(_repository, _visibility).CanSendFriendRequest(Id(_actor), Id(_target));
        decision.Denial.Should().Be(SocialDenialKind.Blocked);
    }

    [Theory]
    [InlineData(SocialVisibility.Nobody)]
    public async Task Policy_Nobody_DeniesFriendRequestAndParty(SocialVisibility visibility)
    {
        _visibility.FriendRequests = visibility;
        _visibility.PartyInvites = visibility;
        var policy = new SocialPolicy(_repository, _visibility);
        (await policy.CanSendFriendRequest(Id(_actor), Id(_target))).Denial.Should().Be(SocialDenialKind.Visibility);
        (await policy.CanInviteToParty(Id(_actor), Id(_target))).Denial.Should().Be(SocialDenialKind.Visibility);
    }

    [Fact]
    public async Task Policy_FriendsOnly_DeniesNonFriendAndAllowsFriend()
    {
        _visibility.FriendRequests = SocialVisibility.FriendsOnly;
        _visibility.PartyInvites = SocialVisibility.FriendsOnly;
        _visibility.Messages = SocialVisibility.FriendsOnly;
        _visibility.OnlineStatus = SocialVisibility.FriendsOnly;
        var policy = new SocialPolicy(_repository, _visibility);

        (await policy.CanSendFriendRequest(Id(_actor), Id(_target))).Allowed.Should().BeFalse();
        (await policy.CanMessage(Id(_actor), Id(_target))).Allowed.Should().BeFalse();
        (await policy.CanViewOnlineStatus(Id(_actor), Id(_target))).Allowed.Should().BeFalse();

        await _repository.AddFriendAsync(Id(_actor), Id(_target));

        (await policy.CanInviteToParty(Id(_actor), Id(_target))).Allowed.Should().BeTrue();
        (await policy.CanMessage(Id(_actor), Id(_target))).Allowed.Should().BeTrue();
        (await policy.CanViewOnlineStatus(Id(_actor), Id(_target))).Allowed.Should().BeTrue();
    }

    [Fact]
    public async Task Policy_Everyone_AllowsUnlessBlocked()
    {
        _visibility.SetAll(SocialVisibility.Everyone);
        var policy = new SocialPolicy(_repository, _visibility);
        (await policy.CanSendFriendRequest(Id(_actor), Id(_target))).Allowed.Should().BeTrue();
        await _repository.AddBlockAsync(Id(_target), Id(_actor));
        (await policy.CanSendFriendRequest(Id(_actor), Id(_target))).Denial.Should().Be(SocialDenialKind.Blocked);
    }

    [Fact]
    public async Task FriendLifecycle_SendAccept_IsBilateral_AndRetryDoesNotDuplicate()
    {
        await _service.SendFriendRequestAsync(Id(_actor), Id(_target));
        await _service.AcceptFriendRequestAsync(Id(_target), Id(_actor));

        (await _repository.GetFriendsAsync(Id(_actor))).Should().Contain(Id(_target));
        (await _repository.GetFriendsAsync(Id(_target))).Should().Contain(Id(_actor));

        await _repository.AddFriendAsync(Id(_actor), Id(_target));
        (await _repository.GetFriendsAsync(Id(_actor))).Count(id => id == Id(_target)).Should().Be(1);
    }

    [Fact]
    public async Task FriendLifecycle_DuplicateSelfAndResolved_AreRejected()
    {
        var actSelf = () => _service.SendFriendRequestAsync(Id(_actor), Id(_actor));
        await actSelf.Should().ThrowAsync<SocialValidationException>();

        await _service.SendFriendRequestAsync(Id(_actor), Id(_target));
        var duplicate = () => _service.SendFriendRequestAsync(Id(_actor), Id(_target));
        await duplicate.Should().ThrowAsync<SocialConflictException>();

        await _service.DeclineFriendRequestAsync(Id(_target), Id(_actor));
        var again = () => _service.AcceptFriendRequestAsync(Id(_target), Id(_actor));
        await again.Should().ThrowAsync<SocialConflictException>();

        var missing = () => _service.CancelFriendRequestAsync(Id(_actor), Id(_target));
        await missing.Should().ThrowAsync<SocialConflictException>();
    }

    [Fact]
    public async Task FriendLifecycle_CancelUsesReceiverId()
    {
        await _service.SendFriendRequestAsync(Id(_actor), Id(_target));
        await _service.CancelFriendRequestAsync(Id(_actor), Id(_target));
        (await _repository.ListPendingFriendRequestsAsync(Id(_actor))).Should().BeEmpty();
    }

    [Fact]
    public async Task Block_RemovesFriendshipAndVoidsPendingInvite()
    {
        await _repository.AddFriendAsync(Id(_actor), Id(_target));
        _visibility.PartyInvites = SocialVisibility.FriendsOnly;
        await _service.SendPartyInviteAsync(Id(_actor), Id(_target));

        await _service.BlockUserAsync(Id(_target), Id(_actor));

        (await _repository.AreFriendsAsync(Id(_actor), Id(_target))).Should().BeFalse();
        (await _repository.ListPendingPartyInvitesAsync(Id(_actor))).Should().BeEmpty();
        var denied = () => _service.SendFriendRequestAsync(Id(_actor), Id(_target));
        await denied.Should().ThrowAsync<SocialPolicyDeniedException>();
    }

    [Fact]
    public async Task PartyLifecycle_NobodyFriendsOnlyAcceptDecline()
    {
        _visibility.PartyInvites = SocialVisibility.Nobody;
        var nobody = () => _service.SendPartyInviteAsync(Id(_actor), Id(_target));
        await nobody.Should().ThrowAsync<SocialPolicyDeniedException>();

        _visibility.PartyInvites = SocialVisibility.FriendsOnly;
        var notFriends = () => _service.SendPartyInviteAsync(Id(_actor), Id(_target));
        await notFriends.Should().ThrowAsync<SocialPolicyDeniedException>();

        await _repository.AddFriendAsync(Id(_actor), Id(_target));
        await _service.SendPartyInviteAsync(Id(_actor), Id(_target));
        await _service.DeclinePartyInviteAsync(Id(_target), Id(_actor));
        var resolved = () => _service.AcceptPartyInviteAsync(Id(_target), Id(_actor));
        await resolved.Should().ThrowAsync<SocialConflictException>();

        await _service.SendPartyInviteAsync(Id(_actor), Id(_target));
        await _service.AcceptPartyInviteAsync(Id(_target), Id(_actor));
        (await _repository.ListPendingPartyInvitesAsync(Id(_target))).Should().BeEmpty();
    }

    [Fact]
    public async Task EnsureUser_SecondCall_DoesNotResetFriends()
    {
        await _repository.EnsureUserAsync(Id(_actor));
        await _repository.AddFriendAsync(Id(_actor), Id(_target));
        await _repository.EnsureUserAsync(Id(_actor));
        (await _repository.GetFriendsAsync(Id(_actor))).Should().ContainSingle().Which.Should().Be(Id(_target));
    }

    [Fact]
    public void Indexes_AreUniqueForUserAndPendingPairs()
    {
        SocialIndexDefinitions.UserIdUnique().Options.Unique.Should().BeTrue();
        SocialIndexDefinitions.PendingFriendPairUnique().Options.Unique.Should().BeTrue();
        SocialIndexDefinitions.PendingPartyPairUnique().Options.Unique.Should().BeTrue();
    }

    [Fact]
    public async Task DuplicateFriendRequest_MapsToConflictProblemDetails()
    {
        await _service.SendFriendRequestAsync(Id(_actor), Id(_target));
        SocialConflictException? conflict = null;
        try
        {
            await _service.SendFriendRequestAsync(Id(_actor), Id(_target));
        }
        catch (SocialConflictException ex)
        {
            conflict = ex;
        }

        conflict.Should().NotBeNull();
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var handled = await new SocialExceptionHandler().TryHandleAsync(context, conflict!, CancellationToken.None);

        handled.Should().BeTrue();
        context.Response.StatusCode.Should().Be(409);
        context.Response.ContentType.Should().Be("application/problem+json");
        context.Response.Body.Position = 0;
        using var document = await JsonDocument.ParseAsync(context.Response.Body);
        document.RootElement.GetProperty("type").GetString().Should().Be("https://pickmeup/errors/version-conflict");
        document.RootElement.GetProperty("status").GetInt32().Should().Be(409);
        document.RootElement.GetProperty("code").GetString().Should().Be("social.request_exists");
    }

    private static string Id(Guid id) => id.ToString("D");

    private sealed class StubVisibility : ISocialVisibilityQuery
    {
        public SocialVisibility FriendRequests { get; set; } = SocialVisibility.Everyone;
        public SocialVisibility PartyInvites { get; set; } = SocialVisibility.Everyone;
        public SocialVisibility Messages { get; set; } = SocialVisibility.Everyone;
        public SocialVisibility OnlineStatus { get; set; } = SocialVisibility.Everyone;

        public void SetAll(SocialVisibility visibility)
        {
            FriendRequests = visibility;
            PartyInvites = visibility;
            Messages = visibility;
            OnlineStatus = visibility;
        }

        public Task<SocialVisibilitySnapshot> GetForAsync(Guid accountId)
            => Task.FromResult(new SocialVisibilitySnapshot(FriendRequests, PartyInvites, Messages, OnlineStatus));
    }
}
