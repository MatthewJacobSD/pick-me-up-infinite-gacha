namespace PickMeUp.Api.Account.AccountPreferences.Audio
{
    // PUT section
    public sealed class AudioDto
    {
        // Main Volume Audio
        public float MasterVolume { get; init; }

        // Music Audio
        public float MusicVolume { get; init; }
        public bool MusicMuted { get; init; }

        // Sound Effects Audio
        public float SfxVolume { get; init; }
        public bool SfxMuted { get; init; }

        // Voice Audio Speaker
        public float VoiceVolume { get; init; }
        public bool VoiceMuted { get; init; }

        // Environemnt Audio
        public float AmbientVolume { get; init; }
        public bool AmbientMuted { get; init; }

        // Version
        public int Version { get; init; }

        public AudioSettings ToSettings() => new()
        {
            MasterVolume = MasterVolume,

            MusicVolume = MusicVolume,
            MusicMuted = MusicMuted,

            SfxVolume = SfxVolume,
            SfxMuted = SfxMuted,

            VoiceVolume = VoiceVolume,
            VoiceMuted = VoiceMuted,

            AmbientVolume = AmbientVolume,
            AmbientMuted = AmbientMuted
        };
    }

    // PATCH section
    public sealed record AudioPatchDto
    {
        public float? MasterVolume { get; init; }

        public float? MusicVolume { get; init; }
        public bool? MusicMuted { get; init; }
        
        public float? SfxVolume { get; init; }
        public bool? SfxMuted { get; init; }
        
        public float? VoiceVolume { get; init; }
        public bool? VoiceMuted { get; init; }
        
        public float? AmbientVolume { get; init; }
        public bool? AmbientMuted { get; init; }
        
        public int Version { get; init; }

        public AudioSettings ApplyTo(AudioSettings current) => current with
        {
            MasterVolume = MasterVolume ?? current.MasterVolume,
        
            MusicVolume = MusicVolume ?? current.MusicVolume,
            MusicMuted = MusicMuted ?? current.MusicMuted,
            
            SfxVolume = SfxVolume ?? current.SfxVolume,
            SfxMuted = SfxMuted ?? current.SfxMuted,
            
            VoiceVolume = VoiceVolume ?? current.VoiceVolume,
            VoiceMuted = VoiceMuted ?? current.VoiceMuted,
            
            AmbientVolume = AmbientVolume ?? current.AmbientVolume,
            AmbientMuted = AmbientMuted ?? current.AmbientMuted
        };
    }

}
