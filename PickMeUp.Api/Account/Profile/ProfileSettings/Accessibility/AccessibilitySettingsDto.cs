namespace PickMeUp.Api.Account.Profile.ProfileSettings.Accessibility
{
    // DTO for incoming accessibility settings updates (PUT requests).
    // No defaults — values come from the client.

    public sealed class AccessibilitySettingsDto
    {
        public string ColorblindMode { get; init; } = string.Empty;
        public bool HighContrastMode { get; init; }

        public bool SubtitlesEnabled { get; init; }
        public int SubtitleSize { get; init; }
        public float SubtitleOpacity { get; init; }
        public bool SubtitleSpeakerNames { get; init; }
        public bool SubtitleSoundEffects { get; init; }

        public bool VisualAudioIndicators { get; init; }
        public bool FootstepVisualization { get; init; }
        public bool GunshotVisualization { get; init; }

        public bool ReducedMotion { get; init; }
        public bool DisableFlashingEffects { get; init; }
        public bool SimplifiedUI { get; init; }
        public int TextSize { get; init; }
    }
}
