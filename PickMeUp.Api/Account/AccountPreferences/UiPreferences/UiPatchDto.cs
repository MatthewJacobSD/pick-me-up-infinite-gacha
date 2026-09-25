namespace PickMeUp.Api.Account.AccountPreferences.UiPreferences;

public sealed record UiPatchDto
{
    public float? UiScale { get; init; }
    public UiTextSize? TextSize { get; init; }
    public UiIconSize? IconSize { get; init; }

    public bool? ShowMinimap { get; init; }
    public bool? ShowChatWindow { get; init; }
    public bool? ShowQuestTracker { get; init; }
    public bool? ShowActionBars { get; init; }

    public HudPosition? MinimapPosition { get; init; }
    public HudPosition? ChatWindowPosition { get; init; }
    public HudPosition? QuestTrackerPosition { get; init; }
    public ActionBarLayout? ActionBarLayout { get; init; }

    public InventoryLayoutMode? InventoryLayout { get; init; }
    public ChatLayoutMode? ChatLayout { get; init; }

    public int Version { get; init; }

    public UiSettings ApplyTo(UiSettings current) => current with
    {
        UiScale = UiScale ?? current.UiScale,
        TextSize = TextSize ?? current.TextSize,
        IconSize = IconSize ?? current.IconSize,

        ShowMinimap = ShowMinimap ?? current.ShowMinimap,
        ShowChatWindow = ShowChatWindow ?? current.ShowChatWindow,
        ShowQuestTracker = ShowQuestTracker ?? current.ShowQuestTracker,
        ShowActionBars = ShowActionBars ?? current.ShowActionBars,

        MinimapPosition = MinimapPosition ?? current.MinimapPosition,
        ChatWindowPosition = ChatWindowPosition ?? current.ChatWindowPosition,
        QuestTrackerPosition = QuestTrackerPosition ?? current.QuestTrackerPosition,
        ActionBarLayout = ActionBarLayout ?? current.ActionBarLayout,

        InventoryLayout = InventoryLayout ?? current.InventoryLayout,
        ChatLayout = ChatLayout ?? current.ChatLayout
    };
}
