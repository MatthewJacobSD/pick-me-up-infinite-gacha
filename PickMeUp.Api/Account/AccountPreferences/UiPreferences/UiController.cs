using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickMeUp.Api.Common.Authentication;

namespace PickMeUp.Api.Account.AccountPreferences.UiPreferences
{
    /// <summary>
    /// Provides full-replace read/write for the UI preferences section of an account.
    /// Covers HUD layout, scale, element visibility, and panel positions.
    /// </summary>
    [ApiController]
    [Route("account/preferences/ui")]
    [Authorize]
    public sealed class UiController(IAccountPreferencesRepository repo, ICurrentUser currentUser) : ControllerBase
    {
        private readonly IAccountPreferencesRepository _repo = repo;
        private readonly ICurrentUser _currentUser = currentUser;

        /**--------[Endpoints]--------**/

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
            return Ok(new { doc.UiPreferences, doc.Version });
        }

        /// <summary>
        /// Replaces the entire UI preferences block, guarded by optimistic-concurrency version check.
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UiPreferencesDto dto)
        {
            var newVersion = await _repo.ReplaceUiPreferencesSettingsAsync(_currentUser.AccountId, dto.ToSettings(), dto.Version);
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
            return Ok(new { doc.UiPreferences, Version = newVersion });
        }
    }
}
