using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickMeUp.Api.Common.Authentication;

namespace PickMeUp.Api.Account.AccountPreferences.Accessibility
{
    [ApiController]
    [Route("account/preferences/accessibility")]
    [Authorize]
    public sealed class AccessibilitySettingsController(IAccountPreferencesRepository repo, ICurrentUser currentUser) : ControllerBase
    {
        private readonly IAccountPreferencesRepository _repo = repo;
        private readonly ICurrentUser _currentUser = currentUser;

        [HttpGet]
        public async Task<IActionResult> Get()
        { // get User Account Accessibility Settings
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
            return Ok(new { doc.Accessibility, doc.Version });
        }

        [HttpPut]
        public async Task<IActionResult> Replace([FromBody] AccessibilitySettingsDto dto)
        {
            // Replace or well, apply new changes to the whole settings
            var newVersion = await _repo.ReplaceAccessibilitySettingsAsync(
                _currentUser.AccountId, dto.ToSettings(), dto.Version);
            
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
            return Ok(new { doc.Accessibility, Version = newVersion });
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] AccessibilitySettingsDto dto)
        {
            // Update target only the small chances, otherwise goes default to Put to update all settings
            var currentVersion = await _repo.UpdateAccessibilitySettingsAsync(
                _currentUser.AccountId, dto.ToSettings(), dto.Version);
            
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
            return Ok(new { doc.Accessibility, Version = currentVersion });
        }

        [HttpPost("reset")]
        public async Task<IActionResult> Reset([FromBody] VersionDto dto)
        {
            // Resets settings to default
            var doc = await _repo.ReplaceAccessibilitySettingsAsync(
                _currentUser.AccountId, AccessibilitySettings.Default, dto.Version);
            
            return Ok(new { doc.Accessibility, doc.Version });
        }

        [HttpGet("deafalts")]
        [AllowAnonymous]
        // Defaults setting to this state
        public IActionResult Defaults() => Ok(AccessibilitySettings.Default);
    }
}
