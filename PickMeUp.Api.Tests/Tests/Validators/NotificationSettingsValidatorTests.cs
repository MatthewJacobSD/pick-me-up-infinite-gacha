using FluentAssertions;
using PickMeUp.Api.Account.AccountPreferences.Notifications;
using Xunit;

namespace PickMeUp.Api.Tests.Tests.Validators;

public class NotificationValidatorTests
{
    private readonly NotificationValidator _sut = new();

    private static NotificationDto Dto(
        bool eventNotifications = false,
        bool friendRequest = false,
        bool partyInvite = false,
        bool systemAnnouncements = false,
        bool rewardNotifications = false,
        int version = 0) => new()
    {
        EventNotifications = eventNotifications,
        FriendRequestNotifications = friendRequest,
        PartyInviteNotifications = partyInvite,
        SystemAnnouncements = systemAnnouncements,
        RewardNotifications = rewardNotifications,
        Version = version
    };

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Validate_EventNotifications_AcceptsBothValues(bool value)
    {
        var dto = Dto(eventNotifications: value);
        _sut.Validate(dto).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Validate_FriendRequestNotifications_AcceptsBothValues(bool value)
    {
        var dto = Dto(friendRequest: value);
        _sut.Validate(dto).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Validate_PartyInviteNotifications_AcceptsBothValues(bool value)
    {
        var dto = Dto(partyInvite: value);
        _sut.Validate(dto).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Validate_SystemAnnouncements_AcceptsBothValues(bool value)
    {
        var dto = Dto(systemAnnouncements: value);
        _sut.Validate(dto).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Validate_RewardNotifications_AcceptsBothValues(bool value)
    {
        var dto = Dto(rewardNotifications: value);
        _sut.Validate(dto).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(100)]
    public void Validate_NonNegativeVersion_ReturnsValid(int version)
    {
        var dto = Dto(version: version);
        _sut.Validate(dto).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_AllBooleansTrue_ReturnsValid()
    {
        var dto = Dto(
            eventNotifications: true,
            friendRequest: true,
            partyInvite: true,
            systemAnnouncements: true,
            rewardNotifications: true);
        _sut.Validate(dto).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_AllBooleansFalse_ReturnsValid()
    {
        var dto = Dto(
            eventNotifications: false,
            friendRequest: false,
            partyInvite: false,
            systemAnnouncements: false,
            rewardNotifications: false);
        _sut.Validate(dto).IsValid.Should().BeTrue();
    }
}
