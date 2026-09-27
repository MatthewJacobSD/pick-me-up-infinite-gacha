using FluentAssertions;
using PickMeUp.Api.Account.AccountPreferences.SocialPreferences;
using Xunit;

namespace PickMeUp.Api.Tests.Tests.Validators;

public class SocialValidatorTests
{
    private readonly SocialValidator _sut = new();

    [Theory]
    [InlineData(SocialVisibility.Everyone)]
    [InlineData(SocialVisibility.FriendsOnly)]
    [InlineData(SocialVisibility.Nobody)]
    public void Validate_ValidFriendRequests_ReturnsValid(SocialVisibility vis)
    {
        var dto = new SocialDto { FriendRequests = vis };
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_InvalidFriendRequests_ReturnsInvalid()
    {
        var dto = new SocialDto { FriendRequests = (SocialVisibility)999 };
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(SocialDto.FriendRequests));
    }

    [Theory]
    [InlineData(SocialVisibility.Everyone)]
    [InlineData(SocialVisibility.FriendsOnly)]
    [InlineData(SocialVisibility.Nobody)]
    public void Validate_ValidMessages_ReturnsValid(SocialVisibility vis)
    {
        var dto = new SocialDto { Messages = vis };
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_InvalidMessages_ReturnsInvalid()
    {
        var dto = new SocialDto { Messages = (SocialVisibility)999 };
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(SocialDto.Messages));
    }

    [Theory]
    [InlineData(SocialVisibility.Everyone)]
    [InlineData(SocialVisibility.FriendsOnly)]
    [InlineData(SocialVisibility.Nobody)]
    public void Validate_ValidPartyInvites_ReturnsValid(SocialVisibility vis)
    {
        var dto = new SocialDto { PartyInvites = vis };
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_InvalidPartyInvites_ReturnsInvalid()
    {
        var dto = new SocialDto { PartyInvites = (SocialVisibility)999 };
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(SocialVisibility.Everyone)]
    [InlineData(SocialVisibility.FriendsOnly)]
    [InlineData(SocialVisibility.Nobody)]
    public void Validate_ValidOnlineStatus_ReturnsValid(SocialVisibility vis)
    {
        var dto = new SocialDto { OnlineStatus = vis };
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_InvalidOnlineStatus_ReturnsInvalid()
    {
        var dto = new SocialDto { OnlineStatus = (SocialVisibility)999 };
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_AllDefaults_ReturnsValid()
    {
        var dto = new SocialDto();
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeTrue();
    }
}
