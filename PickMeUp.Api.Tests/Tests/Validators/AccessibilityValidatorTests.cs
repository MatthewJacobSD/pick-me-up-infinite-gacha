using FluentAssertions;
using PickMeUp.Api.Account.AccountPreferences.Accessibility;
using Xunit;

namespace PickMeUp.Api.Tests.Tests.Validators;

public class AccessibilityValidatorTests
{
    private readonly AccessibilityValidator _sut = new();

    private static AccessibilityDto Dto(
        ColorBlindType colorBlindMode = ColorBlindType.None,
        int subtitleSize = 20,
        float subtitleOpacity = 1f,
        int textSize = 20,
        int version = 0) => new()
    {
        ColorBlindMode = colorBlindMode,
        SubtitleSize = subtitleSize,
        SubtitleOpacity = subtitleOpacity,
        TextSize = textSize,
        Version = version
    };

    // ── ColorBlindMode ──

    [Theory]
    [InlineData(ColorBlindType.None)]
    [InlineData(ColorBlindType.Protonopia)]
    [InlineData(ColorBlindType.Deuteranopia)]
    [InlineData(ColorBlindType.Tritanopia)]
    public void Validate_ValidColorBlindMode_ReturnsValid(ColorBlindType mode)
    {
        var dto = Dto(colorBlindMode: mode);
        _sut.Validate(dto).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_InvalidColorBlindMode_ReturnsInvalid()
    {
        var dto = Dto(colorBlindMode: (ColorBlindType)99);
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(AccessibilityDto.ColorBlindMode));
    }

    // ── SubtitleSize ──

    [Theory]
    [InlineData(10)]
    [InlineData(25)]
    [InlineData(40)]
    public void Validate_ValidSubtitleSize_ReturnsValid(int size)
    {
        var dto = Dto(subtitleSize: size);
        _sut.Validate(dto).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(9)]
    [InlineData(41)]
    [InlineData(0)]
    public void Validate_InvalidSubtitleSize_ReturnsInvalid(int size)
    {
        var dto = Dto(subtitleSize: size);
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(AccessibilityDto.SubtitleSize));
    }

    // ── SubtitleOpacity ──

    [Theory]
    [InlineData(0f)]
    [InlineData(0.5f)]
    [InlineData(1f)]
    public void Validate_ValidSubtitleOpacity_ReturnsValid(float opacity)
    {
        var dto = Dto(subtitleOpacity: opacity);
        _sut.Validate(dto).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(-0.1f)]
    [InlineData(1.1f)]
    public void Validate_InvalidSubtitleOpacity_ReturnsInvalid(float opacity)
    {
        var dto = Dto(subtitleOpacity: opacity);
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(AccessibilityDto.SubtitleOpacity));
    }

    // ── TextSize ──

    [Theory]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(40)]
    public void Validate_ValidTextSize_ReturnsValid(int size)
    {
        var dto = Dto(textSize: size);
        _sut.Validate(dto).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(9)]
    [InlineData(41)]
    [InlineData(0)]
    public void Validate_InvalidTextSize_ReturnsInvalid(int size)
    {
        var dto = Dto(textSize: size);
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(AccessibilityDto.TextSize));
    }

    // ── Version ──

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public void Validate_NonNegativeVersion_ReturnsValid(int version)
    {
        var dto = Dto(version: version);
        _sut.Validate(dto).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_NegativeVersion_ReturnsInvalid()
    {
        var dto = Dto(version: -1);
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(AccessibilityDto.Version));
    }

    // ── Defaults ──

    [Fact]
    public void Validate_DefaultDto_ReturnsValid()
    {
        var dto = Dto();
        _sut.Validate(dto).IsValid.Should().BeTrue();
    }
}
