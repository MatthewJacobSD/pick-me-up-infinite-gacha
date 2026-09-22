using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickMeUp.Api.Account.AccountPreferences;

namespace PickMeUp.Api.Account.AccountPreferences.Accessibility
{
    // ── Accessibility Settings Controller ──────────────
    // REST endpoints for accessibility preferences.
    //
    // 1. GET  /account/preferences/accessibility  — returns current settings
    // 2. PUT  /account/preferences/accessibility  — updates settings

    [ApiController]
    [Route("account/preferences/accessibility")]
    [Authorize]
    public sealed class AccessibilitySettingsController : ControllerBase
    {
        private readonly IAccountPreferencesRepository _repo;

        public AccessibilitySettingsController(IAccountPreferencesRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = User.Identity!.Name!;
            var prefs = await _repo.GetAccessibilitySettingsAsync(userId);
            return Ok(prefs);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AccessibilitySettingsDto dto)
        {
            var userId = User.Identity!.Name!;
            var settings = new AccessibilitySettings
            {
                ColorblindMode = dto.ColorblindMode,
                HighContrastMode = dto.HighContrastMode,

                SubtitlesEnabled = dto.SubtitlesEnabled,
                SubtitleSize = dto.SubtitleSize,
                SubtitleOpacity = dto.SubtitleOpacity,
                SubtitleSpeakerNames = dto.SubtitleSpeakerNames,
                SubtitleSoundEffects = dto.SubtitleSoundEffects,

                VisualAudioIndicators = dto.VisualAudioIndicators,
                FootstepVisualization = dto.FootstepVisualization,
                GunshotVisualization = dto.GunshotVisualization,

                ReducedMotion = dto.ReducedMotion,
                DisableFlashingEffects = dto.DisableFlashingEffects,
                SimplifiedUI = dto.SimplifiedUI,
                TextSize = dto.TextSize
            };

            await _repo.UpdateAccessibilitySettingsAsync(userId, settings);
            return Ok();
        }
    }
}
