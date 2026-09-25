namespace PickMeUp.Api.Account.AccountPreferences.Gameplay
{
    // --------------- [Gameplay Settings] --------------------
    public sealed record GameplaySettings
    {
        public static GameplaySettings Default { get; } = new();
        // Action Bars
        public int VisibleActionBars { get; init; } = 1;
        public bool ShowCooldownNumbers { get; init; } = true;
        public bool ShowKeybindLabels { get; init; } = true;
        public bool LockActionBars { get; init; } = false;

        // Combat UI 
        public bool ShowDamageNumbers { get; init; } = true;
        public bool ShowHealingNumbers { get; init; } = true;
        public bool ShowCriticalEffects { get; init; } = true;
        public bool ShowFloatingCombatText { get; init; } = true;

        // Camera
        public bool InvertYAxis { get; init; } = false;
        public bool InvertXAxis { get; init; } = false;
        public float CameraSensitivity { get; init; } = 1.0f;
        public float FieldOfView { get; init; } = 75f;

        // Automation, interaction
        public bool AutoLoot { get; init; } = true;
        public bool HighlightInteractables { get; init; } = true;

        // Nameplates, pings
        public bool ShowNameplates { get; init; } = true;
        public bool ShowEnemyNameplates { get; init; } = true;
        public bool ShowFriendlyNameplates { get; init; } = true;
        public bool ShowPingIndicators { get; init; } = true;

        // Movement
        public bool AutoRunToggle { get; init; } = false;
        public bool ToggleSprint { get; init; } = false;
    }
}
