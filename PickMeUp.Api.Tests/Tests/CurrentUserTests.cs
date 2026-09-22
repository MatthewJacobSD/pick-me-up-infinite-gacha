using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using PickMeUp.Api.Common.Authentication;
using Xunit;

namespace PickMeUp.Api.Tests.Tests;

public class CurrentUserTests
{
    private readonly Mock<IHttpContextAccessor> _accessorMock = new();

    private CurrentUser CreateSut(ClaimsPrincipal? user = null)
    {
        var ctx = new DefaultHttpContext { User = user ?? new ClaimsPrincipal() };
        _accessorMock.Setup(a => a.HttpContext).Returns(ctx);
        return new CurrentUser(_accessorMock.Object);
    }

    [Fact]
    public void AccountId_FromSubClaim_ReturnsGuid()
    {
        var guid = Guid.NewGuid();
        var identity = new ClaimsIdentity(new[] { new Claim("sub", guid.ToString()) }, "test");
        var sut = CreateSut(new ClaimsPrincipal(identity));

        sut.AccountId.Should().Be(guid);
    }

    [Fact]
    public void AccountId_FromNameIdentifierClaim_ReturnsGuid()
    {
        var guid = Guid.NewGuid();
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, guid.ToString())
        }, "test");
        var sut = CreateSut(new ClaimsPrincipal(identity));

        sut.AccountId.Should().Be(guid);
    }

    [Fact]
    public void AccountId_FromAccountIdClaim_ReturnsGuid()
    {
        var guid = Guid.NewGuid();
        var identity = new ClaimsIdentity(new[] { new Claim("accountId", guid.ToString()) }, "test");
        var sut = CreateSut(new ClaimsPrincipal(identity));

        sut.AccountId.Should().Be(guid);
    }

    [Fact]
    public void AccountId_PrefersNameIdentifierOverSub()
    {
        var nameIdGuid = Guid.NewGuid();
        var subGuid = Guid.NewGuid();
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, nameIdGuid.ToString()),
            new Claim("sub", subGuid.ToString())
        }, "test");
        var sut = CreateSut(new ClaimsPrincipal(identity));

        sut.AccountId.Should().Be(nameIdGuid);
    }

    [Fact]
    public void AccountId_InvalidGuid_ReturnsEmpty()
    {
        var identity = new ClaimsIdentity(new[] { new Claim("sub", "not-a-guid") }, "test");
        var sut = CreateSut(new ClaimsPrincipal(identity));

        sut.AccountId.Should().Be(Guid.Empty);
    }

    [Fact]
    public void AccountId_MissingClaim_ReturnsEmpty()
    {
        var identity = new ClaimsIdentity(Array.Empty<Claim>(), "test");
        var sut = CreateSut(new ClaimsPrincipal(identity));

        sut.AccountId.Should().Be(Guid.Empty);
    }

    [Fact]
    public void AccountId_NullHttpContext_ReturnsEmpty()
    {
        _accessorMock.Setup(a => a.HttpContext).Returns((HttpContext?)null);
        var sut = new CurrentUser(_accessorMock.Object);

        sut.AccountId.Should().Be(Guid.Empty);
    }

    [Fact]
    public void AccountId_UnauthenticatedUser_ReturnsEmpty()
    {
        var identity = new ClaimsIdentity(Array.Empty<Claim>());
        var sut = CreateSut(new ClaimsPrincipal(identity));

        sut.AccountId.Should().Be(Guid.Empty);
    }

    [Fact]
    public void IsAuthenticated_WithValidGuid_ReturnsTrue()
    {
        var guid = Guid.NewGuid();
        var identity = new ClaimsIdentity(new[] { new Claim("sub", guid.ToString()) }, "test");
        var sut = CreateSut(new ClaimsPrincipal(identity));

        sut.IsAuthenticated.Should().BeTrue();
    }

    [Fact]
    public void IsAuthenticated_Unauthenticated_ReturnsFalse()
    {
        var identity = new ClaimsIdentity(Array.Empty<Claim>());
        var sut = CreateSut(new ClaimsPrincipal(identity));

        sut.IsAuthenticated.Should().BeFalse();
    }

    [Fact]
    public void IsAuthenticated_EmptyGuid_ReturnsFalse()
    {
        var identity = new ClaimsIdentity(new[] { new Claim("sub", "invalid") }, "test");
        var sut = CreateSut(new ClaimsPrincipal(identity));

        sut.IsAuthenticated.Should().BeFalse();
    }
}
