using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickMeUp.Api.Common.Authentication;

namespace PickMeUp.Api.Account.AccountPreferences.Audio
{
    [ApiController]
    [Route("account/preferences/audio")]
    [Authorize]
    public sealed class AudioController(
        IAccountPreferencesRepository repo, ICurrentUser currentUser) : ControllerBase
    {
        private readonly IAccountPreferencesRepository _repo = repo;
        private readonly ICurrentUser _currentUser = currentUser;

        [HttpGet]
        public async Task<IActionResult> Get()
        { // current audio
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
            
            return Ok(new { doc.Audio, doc.Version });
        }

        [HttpPut]
        public async Task<IActionResult> Replace([FromBody] AudioDto dto)
        {
            var doc = await _repo.ReplaceAudioPreferencesSettingsAsync(
                _currentUser.AccountId, dto.ToSettings(), dto.Version);
            
            return Ok(new { doc.Audio, doc.Version });
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] AudioPatchDto patch)
        {
            var doc = await _repo.PatchAudioPreferencesSettingsAsync(
                _currentUser.AccountId, patch, patch.Version);

            return Ok(new { doc.Audio, doc.Version });
        }

        [HttpPost("reset")]
        public async Task<IActionResult> Reset([FromBody] VersionDto dto)
        {
            var doc = await _repo.ReplaceAudioPreferencesSettingsAsync(
                _currentUser.AccountId, AudioSettings.Default, dto.Version);
            
            return Ok(new { doc.Audio, doc.Version });
        }

        [HttpGet("defaults")]
        [AllowAnonymous]
        public IActionResult Defaults() => Ok(AudioSettings.Default);
    }
}