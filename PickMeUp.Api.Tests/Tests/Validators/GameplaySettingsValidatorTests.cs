using FluentAssertions;
using PickMeUp.Api.Account.AccountPreferences.Gameplay;
using Xunit;

namespace PickMeUp.Api.Tests.Tests.Validators;

public class GameplaySettingsValidatorTests
{
    private readonly GameplaySettingsValidator _sut = new();

    private static GameplaySettingsDto ValidDto(int? actionBars = null, float? sensitivity = null, float? fov = null) => new()
    {
        VisibleActionBars = actionBars ?? 3,
        CameraSensitivity = sensitivity ?? 1.0f,
        FieldOfView = fov ?? 75f
    };

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(6)]
    public void Validate_ValidActionBars_ReturnsValid(int bars)
    {
        var dto = ValidDto(actionBars: bars);
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(7)]
    [InlineData(-1)]
    public void Validate_InvalidActionBars_ReturnsInvalid(int bars)
    {
        var dto = ValidDto(actionBars: bars);
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(GameplaySettingsDto.VisibleActionBars));
    }

    [Theory]
    [InlineData(0.1f)]
    [InlineData(5f)]
    [InlineData(10f)]
    public void Validate_ValidSensitivity_ReturnsValid(float sens)
    {
        var dto = ValidDto(sensitivity: sens);
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0.0f)]
    [InlineData(10.1f)]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    public void Validate_InvalidSensitivity_ReturnsInvalid(float sens)
    {
        var dto = ValidDto(sensitivity: sens);
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(60f)]
    [InlineData(90f)]
    [InlineData(120f)]
    public void Validate_ValidFov_ReturnsValid(float fov)
    {
        var dto = ValidDto(fov: fov);
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(59.9f)]
    [InlineData(120.1f)]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    public void Validate_InvalidFov_ReturnsInvalid(float fov)
    {
        var dto = ValidDto(fov: fov);
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_ValidDefaults_ReturnsValid()
    {
        var dto = ValidDto();
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeTrue();
    }
}
