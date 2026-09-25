using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickMeUp.Api.Account.AccountPreferences;
using PickMeUp.Api.Common.Authentication;

namespace PickMeUp.Api.Account.AccountPreferences.UiPreferences
{
    [ApiController]
    [Route("account/preferences/ui")]
    [Authorize]
    public sealed class UiController : ControllerBase
    {
        private readonly IAccountPreferencesRepository _repo;
        private readonly ICurrentUser _currentUser;

        public UiController(IAccountPreferencesRepository repo, ICurrentUser currentUser)
        {
            _repo = repo;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
            return Ok(new { doc.UiPreferences, doc.Version });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UiPreferencesDto dto)
        {
            var newVersion = await _repo.ReplaceUiPreferencesSettingsAsync(_currentUser.AccountId, dto.ToSettings(), dto.Version);
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
            return Ok(new { doc.UiPreferences, Version = newVersion });
        }
    }
}
