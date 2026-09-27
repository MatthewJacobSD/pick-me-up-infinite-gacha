using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickMeUp.Api.Common.Authentication;

namespace PickMeUp.Api.Account.AccountPreferences.Audio
{
    /// <summary>
    /// Full CRUD + reset controller for audio preferences.
    /// Supports PUT (full replace), PATCH (partial update), POST reset (restore defaults),
    /// and an anonymous GET defaults endpoint for clients that need the schema before login.
    /// </summary>
    [ApiController]
    [Route("account/preferences/audio")]
    [Authorize]
    public sealed class AudioController(
        IAccountPreferencesRepository repo, ICurrentUser currentUser) : ControllerBase
    {
        private readonly IAccountPreferencesRepository _repo = repo;
        private readonly ICurrentUser _currentUser = currentUser;

        /**--------[Endpoints]--------**/

        [HttpGet]
        public async Task<IActionResult> Get()
        { // current audio
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
             
            return Ok(new { doc.Audio, doc.Version });
        }

        /// <summary>
        /// Replaces the entire audio settings block, guarded by optimistic-concurrency version check.
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Replace([FromBody] AudioDto dto)
        {
            var doc = await _repo.ReplaceAudioPreferencesSettingsAsync(
                _currentUser.AccountId, dto.ToSettings(), dto.Version);
             
            return Ok(new { doc.Audio, doc.Version });
        }

        /// <summary>
        /// Merges only the provided fields into the current audio settings.
        /// </summary>
        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] AudioPatchDto patch)
        {
            var doc = await _repo.PatchAudioPreferencesSettingsAsync(
                _currentUser.AccountId, patch, patch.Version);

            return Ok(new { doc.Audio, doc.Version });
        }

        /// <summary>
        /// Resets audio preferences back to factory defaults. Requires a valid version for concurrency safety.
        /// </summary>
        [HttpPost("reset")]
        public async Task<IActionResult> Reset([FromBody] VersionDto dto)
        {
            var doc = await _repo.ReplaceAudioPreferencesSettingsAsync(
                _currentUser.AccountId, AudioSettings.Default, dto.Version);
             
            return Ok(new { doc.Audio, doc.Version });
        }

        /// <summary>
        /// Returns the default audio settings. Anonymous — no auth required.
        /// </summary>
        [HttpGet("defaults")]
        [AllowAnonymous]
        public IActionResult Defaults() => Ok(AudioSettings.Default);
    }
}