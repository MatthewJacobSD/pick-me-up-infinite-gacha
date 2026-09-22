using FluentAssertions;
using PickMeUp.Api.Account.AccountPreferences.Audio;
using Xunit;

namespace PickMeUp.Api.Tests.Tests.Validators;

public class AudioPreferencesValidatorTests
{
    private readonly AudioPreferencesValidator _sut = new();

    [Theory]
    [InlineData(0f)]
    [InlineData(0.5f)]
    [InlineData(1f)]
    public void Validate_ValidMasterVolume_ReturnsValid(float vol)
    {
        var dto = new AudioPreferencesDto { MasterVolume = vol };
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(-0.1f)]
    [InlineData(1.1f)]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    public void Validate_InvalidMasterVolume_ReturnsInvalid(float vol)
    {
        var dto = new AudioPreferencesDto { MasterVolume = vol };
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(AudioPreferencesDto.MasterVolume));
    }

    [Theory]
    [InlineData(0f)]
    [InlineData(0.5f)]
    [InlineData(1f)]
    public void Validate_ValidMusicVolume_ReturnsValid(float vol)
    {
        var dto = new AudioPreferencesDto { MusicVolume = vol };
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(-1f)]
    [InlineData(2f)]
    [InlineData(float.NaN)]
    public void Validate_InvalidMusicVolume_ReturnsInvalid(float vol)
    {
        var dto = new AudioPreferencesDto { MusicVolume = vol };
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(0f)]
    [InlineData(0.5f)]
    [InlineData(1f)]
    public void Validate_ValidSfxVolume_ReturnsValid(float vol)
    {
        var dto = new AudioPreferencesDto { SfxVolume = vol };
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(-0.1f)]
    [InlineData(1.1f)]
    public void Validate_InvalidSfxVolume_ReturnsInvalid(float vol)
    {
        var dto = new AudioPreferencesDto { SfxVolume = vol };
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(0f)]
    [InlineData(0.5f)]
    [InlineData(1f)]
    public void Validate_ValidVoiceVolume_ReturnsValid(float vol)
    {
        var dto = new AudioPreferencesDto { VoiceVolume = vol };
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(-0.1f)]
    [InlineData(1.1f)]
    [InlineData(float.PositiveInfinity)]
    public void Validate_InvalidVoiceVolume_ReturnsInvalid(float vol)
    {
        var dto = new AudioPreferencesDto { VoiceVolume = vol };
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(0f)]
    [InlineData(0.5f)]
    [InlineData(1f)]
    public void Validate_ValidAmbientVolume_ReturnsValid(float vol)
    {
        var dto = new AudioPreferencesDto { AmbientVolume = vol };
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(-0.1f)]
    [InlineData(1.1f)]
    [InlineData(float.NaN)]
    public void Validate_InvalidAmbientVolume_ReturnsInvalid(float vol)
    {
        var dto = new AudioPreferencesDto { AmbientVolume = vol };
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_AllDefaults_ReturnsValid()
    {
        var dto = new AudioPreferencesDto();
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeTrue();
    }
}
