namespace PickMeUp.Api.Account.AccountPreferences.Audio
{
    public sealed class AudioPreferencesDto
    {
        public float MasterVolume { get; init; }

        public float MusicVolume { get; init; }
        public bool MusicMuted { get; init; }

        public float SfxVolume { get; init; }
        public bool SfxMuted { get; init; }

        public float VoiceVolume { get; init; }
        public bool VoiceMuted { get; init; }

        public float AmbientVolume { get; init; }
        public bool AmbientMuted { get; init; }

        public int Version { get; init; }

        public AudioPreferencesSettings ToSettings() => new()
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
}
