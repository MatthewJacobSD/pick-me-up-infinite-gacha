namespace PickMeUp.Api.Account.AccountPreferences.Gameplay
{
    // ── Gameplay Settings ──────────────────────────────
    // How the player wants the game to behave.
    // Account-owned, server-persisted, synced across devices.

    public sealed class GameplaySettings
    {
        // ── Action Bars ────────────────────────────────
        public int VisibleActionBars { get; init; } = 1;
        public bool ShowCooldownNumbers { get; init; } = true;
        public bool ShowKeybindLabels { get; init; } = true;
        public bool LockActionBars { get; init; } = false;

        // ── Combat Presentation ────────────────────────
        public bool ShowDamageNumbers { get; init; } = true;
        public bool ShowHealingNumbers { get; init; } = true;
        public bool ShowCriticalEffects { get; init; } = true;
        public bool ShowFloatingCombatText { get; init; } = true;

        // ── Camera ─────────────────────────────────────
        public bool InvertYAxis { get; init; } = false;
        public bool InvertXAxis { get; init; } = false;
        public float CameraSensitivity { get; init; } = 1.0f;
        public float FieldOfView { get; init; } = 75f;

        // ── Interaction ────────────────────────────────
        public bool AutoLoot { get; init; } = true;
        public bool HighlightInteractables { get; init; } = true;

        // ── Nameplates / Pings ─────────────────────────
        public bool ShowNameplates { get; init; } = true;
        public bool ShowEnemyNameplates { get; init; } = true;
        public bool ShowFriendlyNameplates { get; init; } = true;
        public bool ShowPingIndicators { get; init; } = true;

        // ── Movement ───────────────────────────────────
        public bool AutoRunToggle { get; init; } = false;
        public bool ToggleSprint { get; init; } = false;
    }
}
