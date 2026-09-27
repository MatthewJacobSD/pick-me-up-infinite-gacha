using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickMeUp.Api.Common.Authentication;

namespace PickMeUp.Api.Account.AccountPreferences.Gameplay
{
    /// <summary>
    /// Provides full-replace read/write for the gameplay preferences section of an account.
    /// </summary>
    [ApiController]
    [Route("account/preferences/gameplay")]
    [Authorize]
    public sealed class GameplayController(IAccountPreferencesRepository repo, ICurrentUser currentUser) : ControllerBase
    {
        private readonly IAccountPreferencesRepository _repo = repo;
        private readonly ICurrentUser _currentUser = currentUser;

        /**--------[Endpoints]--------**/

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
            return Ok(new { doc.Gameplay, doc.Version });
        }

        /// <summary>
        /// Replaces the entire gameplay settings block, guarded by optimistic-concurrency version check.
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] GameplayDto dto)
        {
            var newVersion = await _repo.ReplaceGameplaySettingsAsync(_currentUser.AccountId, dto.ToSettings(), dto.Version);
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
            return Ok(new { doc.Gameplay, Version = newVersion });
        }
    }
}
