namespace PickMeUp.Api.Account.AccountPreferences.Accessibility
{
    // ── Accessibility Settings ─────────────────────────
    // Personal settings that follow the player.
    // Account-owned, server-persisted, synced across devices.

    public sealed class AccessibilitySettings
    {
        // ── Colors ─────────────────────────────────────
        public string ColorblindMode { get; init; } = "None"; // None, Protanopia, Deuteranopia, Tritanopia
        public bool HighContrastMode { get; init; } = false;

        // ── Subtitles ──────────────────────────────────
        public bool SubtitlesEnabled { get; init; } = true;
        public int SubtitleSize { get; init; } = 16;
        public float SubtitleOpacity { get; init; } = 0.5f;
        public bool SubtitleSpeakerNames { get; init; } = true;
        public bool SubtitleSoundEffects { get; init; } = true;

        // ── Audio Assistance ───────────────────────────
        public bool VisualAudioIndicators { get; init; } = false;
        public bool FootstepVisualization { get; init; } = false;
        public bool GunshotVisualization { get; init; } = false;

        // ── General ────────────────────────────────────
        public bool ReducedMotion { get; init; } = false;
        public bool DisableFlashingEffects { get; init; } = false;
        public bool SimplifiedUI { get; init; } = false;
        public int TextSize { get; init; } = 16;
    }
}
