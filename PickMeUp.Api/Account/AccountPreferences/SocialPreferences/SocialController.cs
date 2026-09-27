using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickMeUp.Api.Common.Authentication;

namespace PickMeUp.Api.Account.AccountPreferences.SocialPreferences
{
    /// <summary>
    /// Provides full-replace read/write for the social preferences section of an account.
    /// Controls visibility of friend requests, messages, party invites, and online status.
    /// </summary>
    [ApiController]
    [Route("account/preferences/social")]
    [Authorize]
    public sealed class SocialController(IAccountPreferencesRepository repo, ICurrentUser currentUser) : ControllerBase
    {
        private readonly IAccountPreferencesRepository _repo = repo;
        private readonly ICurrentUser _currentUser = currentUser;

        /**--------[Endpoints]--------**/

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
            return Ok(new { doc.SocialPreferences, doc.Version });
        }

        /// <summary>
        /// Replaces the entire social settings block, guarded by optimistic-concurrency version check.
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SocialDto dto)
        {
            var newVersion = await _repo.ReplaceSocialPreferencesSettingsAsync(_currentUser.AccountId, dto.ToSettings(), dto.Version);
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
            return Ok(new { doc.SocialPreferences, Version = newVersion });
        }
    }
}
