using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickMeUp.Api.Common.Authentication;

namespace PickMeUp.Api.Account.AccountPreferences.Accessibility
{
    [ApiController]
    [Route("account/preferences/accessibility")]
    [Authorize]
    public sealed class AccessibilityController(
        IAccountPreferencesRepository repo, 
        ICurrentUser currentUser
        ) : ControllerBase
    {
        private readonly IAccountPreferencesRepository _repo = repo;
        private readonly ICurrentUser _currentUser = currentUser;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // confirm it exists if not create
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);

            return Ok(new { doc.Accessibility, doc.Version });
        }

        [HttpPut]
        public async Task<IActionResult> Replace([FromBody] AccessibilityDto dto)
        {
            var doc = await _repo.ReplaceAccessibilitySettingsAsync(
                _currentUser.AccountId, dto.ToSettings(), dto.Version);
            
            return Ok(new { doc.Accessibility, doc.Version });
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] AccessibilityPatchDto patch)
        {
            var doc = await _repo.PatchAccessibilitySettingsAsync(
                _currentUser.AccountId, patch, patch.Version);

            return Ok(new { doc.Accessibility, doc.Version });
        }

        [HttpPost("reset")]
        public async Task<IActionResult> Reset([FromBody] VersionDto dto)
        {
            var doc = await _repo.ReplaceAccessibilitySettingsAsync(
                _currentUser.AccountId, AccessibilitySettings.Default, dto.Version);
            
            return Ok(new { doc.Accessibility, doc.Version });
        }

        [HttpGet("defaults")]
        [AllowAnonymous]
        public IActionResult Defaults() => Ok(AccessibilitySettings.Default);
    }
}
