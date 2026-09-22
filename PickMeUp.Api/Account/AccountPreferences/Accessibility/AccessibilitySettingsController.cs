using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickMeUp.Api.Account.AccountPreferences;
using PickMeUp.Api.Common.Authentication;

namespace PickMeUp.Api.Account.AccountPreferences.Accessibility
{
    [ApiController]
    [Route("account/preferences/accessibility")]
    [Authorize]
    public sealed class AccessibilitySettingsController : ControllerBase
    {
        private readonly IAccountPreferencesRepository _repo;
        private readonly ICurrentUser _currentUser;

        public AccessibilitySettingsController(IAccountPreferencesRepository repo, ICurrentUser currentUser)
        {
            _repo = repo;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
            return Ok(new { doc.Accessibility, doc.Version });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AccessibilitySettingsDto dto)
        {
            var newVersion = await _repo.UpdateAccessibilitySettingsAsync(_currentUser.AccountId, dto.ToSettings(), dto.Version);
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
            return Ok(new { doc.Accessibility, Version = newVersion });
        }
    }
}
