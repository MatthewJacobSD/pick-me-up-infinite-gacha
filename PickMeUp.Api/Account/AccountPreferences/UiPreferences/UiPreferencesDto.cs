namespace PickMeUp.Api.Account.AccountPreferences.UiPreferences
{
    public sealed class UiPreferencesDto
    {
        public float UiScale { get; init; }
        public UiTextSize TextSize { get; init; }
        public UiIconSize IconSize { get; init; }

        public bool ShowMinimap { get; init; }
        public bool ShowChatWindow { get; init; }
        public bool ShowQuestTracker { get; init; }
        public bool ShowActionBars { get; init; }

        public HudPosition MinimapPosition { get; init; }
        public HudPosition ChatWindowPosition { get; init; }
        public HudPosition QuestTrackerPosition { get; init; }
        public ActionBarLayout ActionBarLayout { get; init; }

        public InventoryLayoutMode InventoryLayout { get; init; }
        public ChatLayoutMode ChatLayout { get; init; }
    }
}
