using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickMeUp.Api.Common.Authentication;

namespace PickMeUp.Api.Account.AccountPreferences.Language
{
    /// <summary>
    /// Provides full-replace read/write for the language preferences section of an account.
    /// </summary>
    [ApiController]
    [Route("account/preferences/language")]
    [Authorize]
    public sealed class LanguageController(IAccountPreferencesRepository repo, ICurrentUser currentUser) : ControllerBase
    {
        private readonly IAccountPreferencesRepository _repo = repo;
        private readonly ICurrentUser _currentUser = currentUser;

        /**--------[Endpoints]--------**/

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
            return Ok(new { doc.Language, doc.Version });
        }

        /// <summary>
        /// Replaces the entire language settings block, guarded by optimistic-concurrency version check.
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] LanguageSettingsDto dto)
        {
            var newVersion = await _repo.ReplaceLanguageSettingsAsync(_currentUser.AccountId, dto.ToSettings(), dto.Version);
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
            return Ok(new { doc.Language, Version = newVersion });
        }
    }
}
