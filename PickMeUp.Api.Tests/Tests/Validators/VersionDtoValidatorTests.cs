using FluentAssertions;
using PickMeUp.Api.Account.AccountPreferences;
using Xunit;

namespace PickMeUp.Api.Tests.Tests.Validators;

public class VersionDtoValidatorTests
{
    private readonly VersionDtoValidator _sut = new();

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(100)]
    public void Validate_NonNegativeVersion_ReturnsValid(int version)
    {
        var dto = new VersionDto { Version = version };
        _sut.Validate(dto).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Validate_NegativeVersion_ReturnsInvalid(int version)
    {
        var dto = new VersionDto { Version = version };
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(VersionDto.Version));
    }
}
