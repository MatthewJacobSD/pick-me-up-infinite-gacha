namespace PickMeUp.Api.Account.AccountPreferences.UiPreferences
{
    public sealed record UiSettings
    {
        public static UiSettings Default { get; } = new();

        // Global UI scale (intent)
        public float UiScale { get; init; } = 1.0f;

        // Text size intent
        public UiTextSize TextSize { get; init; } = UiTextSize.Medium;

        // Icon size intent
        public UiIconSize IconSize { get; init; } = UiIconSize.Medium;

        // HUD visibility
        public bool ShowMinimap { get; init; } = true;
        public bool ShowChatWindow { get; init; } = true;
        public bool ShowQuestTracker { get; init; } = true;
        public bool ShowActionBars { get; init; } = true;

        // HUD layout intent (not pixel positions)
        public HudPosition MinimapPosition { get; init; } = HudPosition.TopRight;
        public HudPosition ChatWindowPosition { get; init; } = HudPosition.BottomLeft;
        public HudPosition QuestTrackerPosition { get; init; } = HudPosition.RightSide;
        public ActionBarLayout ActionBarLayout { get; init; } = ActionBarLayout.Horizontal;

        // Inventory layout intent
        public InventoryLayoutMode InventoryLayout { get; init; } = InventoryLayoutMode.Grid;

        // Chat layout intent
        public ChatLayoutMode ChatLayout { get; init; } = ChatLayoutMode.Expanded;
    }

    public enum UiTextSize { Small, Medium, Large }
    public enum UiIconSize { Small, Medium, Large }

    public enum HudPosition
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        LeftSide,
        RightSide,
        Center
    }

    public enum ActionBarLayout
    {
        Horizontal,
        Vertical
    }

    public enum InventoryLayoutMode
    {
        Grid,
        List
    }

    public enum ChatLayoutMode
    {
        Compact,
        Expanded
    }
}
