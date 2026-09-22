using FluentAssertions;
using PickMeUp.Api.Account.AccountPreferences.UiPreferences;
using Xunit;

namespace PickMeUp.Api.Tests.Tests.Validators;

public class UiPreferencesValidatorTests
{
    private readonly UiPreferencesValidator _sut = new();

    private static UiPreferencesDto ValidDto(
        float? scale = null,
        UiTextSize? textSize = null,
        UiIconSize? iconSize = null,
        HudPosition? minimap = null,
        HudPosition? chat = null,
        HudPosition? quest = null,
        ActionBarLayout? actionBar = null,
        InventoryLayoutMode? inventory = null,
        ChatLayoutMode? chatLayout = null) => new()
    {
        UiScale = scale ?? 1.0f,
        TextSize = textSize ?? UiTextSize.Medium,
        IconSize = iconSize ?? UiIconSize.Medium,
        MinimapPosition = minimap ?? HudPosition.TopRight,
        ChatWindowPosition = chat ?? HudPosition.BottomLeft,
        QuestTrackerPosition = quest ?? HudPosition.RightSide,
        ActionBarLayout = actionBar ?? ActionBarLayout.Horizontal,
        InventoryLayout = inventory ?? InventoryLayoutMode.Grid,
        ChatLayout = chatLayout ?? ChatLayoutMode.Expanded
    };

    [Theory]
    [InlineData(0.5f)]
    [InlineData(1.0f)]
    [InlineData(1.5f)]
    [InlineData(2.0f)]
    public void Validate_ValidUiScale_ReturnsValid(float scale)
    {
        var dto = ValidDto(scale: scale);
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0.4f)]
    [InlineData(2.1f)]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    public void Validate_InvalidUiScale_ReturnsInvalid(float scale)
    {
        var dto = ValidDto(scale: scale);
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(UiPreferencesDto.UiScale));
    }

    [Theory]
    [InlineData(UiTextSize.Small)]
    [InlineData(UiTextSize.Medium)]
    [InlineData(UiTextSize.Large)]
    public void Validate_ValidTextSize_ReturnsValid(UiTextSize size)
    {
        var dto = ValidDto(textSize: size);
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_InvalidTextSize_ReturnsInvalid()
    {
        var dto = ValidDto(textSize: (UiTextSize)999);
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(UiPreferencesDto.TextSize));
    }

    [Theory]
    [InlineData(UiIconSize.Small)]
    [InlineData(UiIconSize.Medium)]
    [InlineData(UiIconSize.Large)]
    public void Validate_ValidIconSize_ReturnsValid(UiIconSize size)
    {
        var dto = ValidDto(iconSize: size);
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_InvalidIconSize_ReturnsInvalid()
    {
        var dto = ValidDto(iconSize: (UiIconSize)999);
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(UiPreferencesDto.IconSize));
    }

    [Theory]
    [InlineData(HudPosition.TopLeft)]
    [InlineData(HudPosition.TopRight)]
    [InlineData(HudPosition.BottomLeft)]
    [InlineData(HudPosition.BottomRight)]
    [InlineData(HudPosition.LeftSide)]
    [InlineData(HudPosition.RightSide)]
    [InlineData(HudPosition.Center)]
    public void Validate_ValidHudPosition_ReturnsValid(HudPosition pos)
    {
        var dto = ValidDto(minimap: pos);
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_InvalidHudPosition_ReturnsInvalid()
    {
        var dto = ValidDto(minimap: (HudPosition)999);
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(ActionBarLayout.Horizontal)]
    [InlineData(ActionBarLayout.Vertical)]
    public void Validate_ValidActionBarLayout_ReturnsValid(ActionBarLayout layout)
    {
        var dto = ValidDto(actionBar: layout);
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_InvalidActionBarLayout_ReturnsInvalid()
    {
        var dto = ValidDto(actionBar: (ActionBarLayout)999);
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(InventoryLayoutMode.Grid)]
    [InlineData(InventoryLayoutMode.List)]
    public void Validate_ValidInventoryLayout_ReturnsValid(InventoryLayoutMode layout)
    {
        var dto = ValidDto(inventory: layout);
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_InvalidInventoryLayout_ReturnsInvalid()
    {
        var dto = ValidDto(inventory: (InventoryLayoutMode)999);
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(ChatLayoutMode.Compact)]
    [InlineData(ChatLayoutMode.Expanded)]
    public void Validate_ValidChatLayout_ReturnsValid(ChatLayoutMode layout)
    {
        var dto = ValidDto(chatLayout: layout);
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_InvalidChatLayout_ReturnsInvalid()
    {
        var dto = ValidDto(chatLayout: (ChatLayoutMode)999);
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_AllDefaults_ReturnsValid()
    {
        var dto = ValidDto();
        var result = _sut.Validate(dto);
        result.IsValid.Should().BeTrue();
    }
}
