using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PickMeUp.Api.Account.AccountPreferences.Audio
{
    [ApiController]
    [Route("account/preferences/audio")]
    [Authorize]
    public sealed class AudioPreferencesController : ControllerBase
    {
        private readonly IAccountPreferencesRepository _repo;

        public AudioPreferencesController(IAccountPreferencesRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = User.Identity!.Name!;
            var prefs = await _repo.GetAudioPreferencesAsync(userId);
            return Ok(prefs);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AudioPreferencesDto dto)
        {
            var userId = User.Identity!.Name!;
            var settings = new AudioPreferencesSettings
            {
                MasterVolume = dto.MasterVolume,

                MusicVolume = dto.MusicVolume,
                MusicMuted = dto.MusicMuted,

                SfxVolume = dto.SfxVolume,
                SfxMuted = dto.SfxMuted,

                VoiceVolume = dto.VoiceVolume,
                VoiceMuted = dto.VoiceMuted,

                AmbientVolume = dto.AmbientVolume,
                AmbientMuted = dto.AmbientMuted
            };

            await _repo.UpdateAudioPreferencesAsync(userId, settings);
            return Ok();
        }
    }
}
