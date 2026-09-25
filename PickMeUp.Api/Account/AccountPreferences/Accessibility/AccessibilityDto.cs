namespace PickMeUp.Api.Account.AccountPreferences.Accessibility
{
    //  PUT section
    public sealed record AccessibilityDto
    {
        // Colours
        public ColorBlindType ColorBlindMode { get; init; } = ColorBlindType.None;
        public bool HighContrastMode { get; init; }

        // Subtitles
        public bool SubtitlesEnabled { get; init; }
        public int SubtitleSize { get; init; }
        public float SubtitleOpacity { get; init; }
        public bool SubtitleSpeakerNames { get; init; }
        public bool SubtitleSoundEffects { get; init; }

        // Player
        public bool VisualAudioIndicators { get; init; }
        public bool FootstepVisualization { get; init; }
        public bool GunshotVisualization { get; init; }
        public bool ReducedMotion { get; init; }
        public bool DisableFlashingEffects { get; init; }

        // UI
        public bool SimplifiedUI { get; init; }
        public int TextSize { get; init; }

        // System
        public int Version { get; init; }

        public AccessibilitySettings ToSettings() => new()
        {
            ColorBlindMode = ColorBlindMode,
            HighContrastMode = HighContrastMode,

            SubtitlesEnabled = SubtitlesEnabled,
            SubtitleSize = SubtitleSize,
            SubtitleOpacity = SubtitleOpacity,
            SubtitleSpeakerNames = SubtitleSpeakerNames,
            SubtitleSoundEffects = SubtitleSoundEffects,

            VisualAudioIndicators = VisualAudioIndicators,
            FootstepVisualization = FootstepVisualization,
            GunshotVisualization = GunshotVisualization,

            ReducedMotion = ReducedMotion,
            DisableFlashingEffects = DisableFlashingEffects,
            SimplifiedUI = SimplifiedUI,
            TextSize = TextSize
        };
    }


    // PATCH section
    public sealed record AccessibilityPatchDto
    {
        public ColorBlindType? ColorBlindMode { get; init; }
        public bool? HighContrastMode { get; init; }
        
        public bool? SubtitlesEnabled { get; init; }
        public int? SubtitleSize { get; init; }
        public float? SubtitleOpacity { get; init; }
        public bool? SubtitleSpeakerNames { get; init; }
        public bool? SubtitleSoundEffects { get; init; }
        
        public bool? VisualAudioIndicators { get; init; }
        public bool? FootstepVisualization { get; init; }
        public bool? GunshotVisualization { get; init; }
        public bool? ReducedMotion { get; init; }
        public bool? DisableFlashingEffects { get; init; }
        
        public bool? SimplifiedUI { get; init; }
        public int? TextSize { get; init; }
        public int Version { get; init; }
        public AccessibilitySettings ApplyTo(AccessibilitySettings current) => current with
        {
            ColorBlindMode = ColorBlindMode ?? current.ColorBlindMode,
            HighContrastMode = HighContrastMode ?? current.HighContrastMode,

            SubtitlesEnabled = SubtitlesEnabled ?? current.SubtitlesEnabled,
            SubtitleSize = SubtitleSize ?? current.SubtitleSize,
            SubtitleOpacity = SubtitleOpacity ?? current.SubtitleOpacity,
            SubtitleSpeakerNames = SubtitleSpeakerNames ?? current.SubtitleSpeakerNames,
            SubtitleSoundEffects = SubtitleSoundEffects ?? current.SubtitleSoundEffects,

            VisualAudioIndicators = VisualAudioIndicators ?? current.VisualAudioIndicators,
            FootstepVisualization = FootstepVisualization ?? current.FootstepVisualization,
            GunshotVisualization = GunshotVisualization ?? current.GunshotVisualization,

            ReducedMotion = ReducedMotion ?? current.ReducedMotion,
            DisableFlashingEffects = DisableFlashingEffects ?? current.DisableFlashingEffects,
            SimplifiedUI = SimplifiedUI ?? current.SimplifiedUI,
            TextSize = TextSize ?? current.TextSize
        };
    }
}
