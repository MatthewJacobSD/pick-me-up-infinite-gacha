namespace PickMeUp.Api.Account.AccountPreferences.Audio
{
    public sealed class AudioPreferencesSettings
    {
        // Master volume (0.0 - 1.0)
        public float MasterVolume { get; init; } = 1.0f;

        // Music
        public float MusicVolume { get; init; } = 0.8f;
        public bool MusicMuted { get; init; } = false;

        // Sound effects
        public float SfxVolume { get; init; } = 0.8f;
        public bool SfxMuted { get; init; } = false;

        // Voice chat
        public float VoiceVolume { get; init; } = 0.8f;
        public bool VoiceMuted { get; init; } = false;

        // Ambient/environment
        public float AmbientVolume { get; init; } = 0.7f;
        public bool AmbientMuted { get; init; } = false;
    }
}
