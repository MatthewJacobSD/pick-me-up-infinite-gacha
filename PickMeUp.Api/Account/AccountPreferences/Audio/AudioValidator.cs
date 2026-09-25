using FluentValidation;

namespace PickMeUp.Api.Account.AccountPreferences.Audio
{
    // -------------------------[Audio Validator]---------------------
    public sealed class AudioValidator : AbstractValidator<AudioDto>
    {
        public AudioValidator()
        {
            RuleFor(x => x.MasterVolume).InclusiveBetween(AudioLimits.MinVolume, AudioLimits.MaxVolume);

            RuleFor(x => x.MusicVolume).InclusiveBetween(AudioLimits.MinVolume, AudioLimits.MaxVolume);
            RuleFor(x => x.SfxVolume).InclusiveBetween(AudioLimits.MinVolume, AudioLimits.MaxVolume);
            RuleFor(x => x.VoiceVolume).InclusiveBetween(AudioLimits.MinVolume, AudioLimits.MaxVolume);
            RuleFor(x => x.AmbientVolume).InclusiveBetween(AudioLimits.MinVolume, AudioLimits.MaxVolume);

            RuleFor(x => x.Version).GreaterThanOrEqualTo(0);
        }
    }

    public sealed class AudioPreferencesPatchValidator : AbstractValidator<AudioPatchDto>
    {
        public AudioPreferencesPatchValidator()
        {
            RuleFor(x => x.MasterVolume).InclusiveBetween(
                AudioLimits.MinVolume, AudioLimits.MaxVolume).When(x => x.MasterVolume.HasValue);
            
            RuleFor(x => x.MusicVolume).InclusiveBetween(
                AudioLimits.MinVolume, AudioLimits.MaxVolume).When(x => x.MusicVolume.HasValue);
            RuleFor(x => x.SfxVolume).InclusiveBetween(
                AudioLimits.MinVolume, AudioLimits.MaxVolume).When(x => x.SfxVolume.HasValue);
            RuleFor(x => x.VoiceVolume).InclusiveBetween(
                AudioLimits.MinVolume, AudioLimits.MaxVolume).When(x => x.VoiceVolume.HasValue);
            RuleFor(x => x.AmbientVolume).InclusiveBetween(
                AudioLimits.MinVolume, AudioLimits.MaxVolume).When(x => x.AmbientVolume.HasValue);
            
            RuleFor(x => x.Version).GreaterThanOrEqualTo(0);
        }
    }

    public static class AudioLimits 
    {
        public const float MinVolume = 0f;
        public const float MaxVolume = 1f;
    }

}
