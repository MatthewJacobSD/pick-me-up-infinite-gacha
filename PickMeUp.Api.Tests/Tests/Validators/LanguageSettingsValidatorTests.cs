using FluentAssertions;
using PickMeUp.Api.Account.AccountPreferences.Language;
using Xunit;

namespace PickMeUp.Api.Tests.Tests.Validators;

public class LanguageSettingsValidatorTests
{
    private readonly LanguageSettingsValidator _sut = new();

    [Theory]
    [InlineData("en")]
    [InlineData("nl")]
    [InlineData("fr")]
    [InlineData("de")]
    [InlineData("it")]
    [InlineData("es")]
    [InlineData("pt")]
    [InlineData("pl")]
    [InlineData("ru")]
    [InlineData("ja")]
    [InlineData("ko")]
    [InlineData("zh")]
    public void Validate_AllowedLanguage_ReturnsValid(string lang)
    {
        var dto = new LanguageSettingsDto { PreferredLanguage = lang };
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("xx")]
    [InlineData("en-US")]
    [InlineData("fr-CA")]
    [InlineData("")]
    [InlineData("xxz")]
    public void Validate_UnknownLanguage_ReturnsInvalid(string lang)
    {
        var dto = new LanguageSettingsDto { PreferredLanguage = lang };
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(LanguageSettingsDto.PreferredLanguage));
    }

    [Fact]
    public void Validate_DefaultLanguage_ReturnsValid()
    {
        var dto = new LanguageSettingsDto();
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeTrue();
    }
}
