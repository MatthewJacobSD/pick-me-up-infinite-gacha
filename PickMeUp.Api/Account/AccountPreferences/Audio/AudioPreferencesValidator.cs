using FluentValidation;

namespace PickMeUp.Api.Account.AccountPreferences.Audio
{
    public sealed class AudioPreferencesValidator : AbstractValidator<AudioPreferencesDto>
    {
        public AudioPreferencesValidator()
        {
            RuleFor(x => x.MasterVolume).InclusiveBetween(0f, 1f);

            RuleFor(x => x.MusicVolume).InclusiveBetween(0f, 1f);
            RuleFor(x => x.SfxVolume).InclusiveBetween(0f, 1f);
            RuleFor(x => x.VoiceVolume).InclusiveBetween(0f, 1f);
            RuleFor(x => x.AmbientVolume).InclusiveBetween(0f, 1f);
        }
    }
}
