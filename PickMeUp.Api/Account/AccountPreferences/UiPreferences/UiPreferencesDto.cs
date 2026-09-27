namespace PickMeUp.Api.Account.AccountPreferences.UiPreferences
{
    /// <summary>
    /// Full-replace DTO for UI preferences (PUT). Every field is required.
    /// </summary>
    public sealed class UiPreferencesDto
    {
        /**--------[Scale & Sizing]--------**/

        public float UiScale { get; init; }
        public UiTextSize TextSize { get; init; }
        public UiIconSize IconSize { get; init; }

        /**--------[Element Visibility]--------**/

        public bool ShowMinimap { get; init; }
        public bool ShowChatWindow { get; init; }
        public bool ShowQuestTracker { get; init; }
        public bool ShowActionBars { get; init; }

        /**--------[Panel Positions & Layouts]--------**/

        public HudPosition MinimapPosition { get; init; }
        public HudPosition ChatWindowPosition { get; init; }
        public HudPosition QuestTrackerPosition { get; init; }
        public ActionBarLayout ActionBarLayout { get; init; }

        public InventoryLayoutMode InventoryLayout { get; init; }
        public ChatLayoutMode ChatLayout { get; init; }

        public int Version { get; init; }

        /// <summary>Maps this DTO to the domain <see cref="UiSettings"/> record.</summary>
        public UiSettings ToSettings() => new()
        {
            UiScale = UiScale,
            TextSize = TextSize,
            IconSize = IconSize,

            ShowMinimap = ShowMinimap,
            ShowChatWindow = ShowChatWindow,
            ShowQuestTracker = ShowQuestTracker,
            ShowActionBars = ShowActionBars,

            MinimapPosition = MinimapPosition,
            ChatWindowPosition = ChatWindowPosition,
            QuestTrackerPosition = QuestTrackerPosition,
            ActionBarLayout = ActionBarLayout,

            InventoryLayout = InventoryLayout,
            ChatLayout = ChatLayout
        };
    }
}
