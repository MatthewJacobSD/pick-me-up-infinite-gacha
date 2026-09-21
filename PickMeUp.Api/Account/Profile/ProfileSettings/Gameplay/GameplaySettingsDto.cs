namespace PickMeUp.Api.Account.Profile.ProfileSettings.Gameplay
{
    // ── Gameplay Settings DTO ──────────────────────────
    // Incoming payload for gameplay settings updates (PUT).
    // No defaults — values come from the client.

    public sealed class GameplaySettingsDto
    {
        public int VisibleActionBars { get; init; }
        public bool ShowCooldownNumbers { get; init; }
        public bool ShowKeybindLabels { get; init; }
        public bool LockActionBars { get; init; }

        public bool ShowDamageNumbers { get; init; }
        public bool ShowHealingNumbers { get; init; }
        public bool ShowCriticalEffects { get; init; }
        public bool ShowFloatingCombatText { get; init; }

        public bool InvertYAxis { get; init; }
        public bool InvertXAxis { get; init; }
        public float CameraSensitivity { get; init; }
        public float FieldOfView { get; init; }

        public bool AutoLoot { get; init; }
        public bool HighlightInteractables { get; init; }

        public bool ShowNameplates { get; init; }
        public bool ShowEnemyNameplates { get; init; }
        public bool ShowFriendlyNameplates { get; init; }
        public bool ShowPingIndicators { get; init; }

        public bool AutoRunToggle { get; init; }
        public bool ToggleSprint { get; init; }
    }
}
