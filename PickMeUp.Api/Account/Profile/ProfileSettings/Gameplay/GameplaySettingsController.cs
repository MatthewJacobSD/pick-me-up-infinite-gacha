using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PickMeUp.Api.Account.Profile.ProfileSettings.Gameplay
{
    // ── Gameplay Settings Controller ───────────────────
    // REST endpoints for gameplay preferences.
    //
    // 1. GET  /account/preferences/gameplay  — returns current settings
    // 2. PUT  /account/preferences/gameplay  — updates settings

    [ApiController]
    [Route("account/preferences/gameplay")]
    [Authorize]
    public sealed class GameplaySettingsController(IAccountPreferencesRepository repo) : ControllerBase
    {
        private readonly IAccountPreferencesRepository _repo = repo;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = User.Identity!.Name!;
            var prefs = await _repo.GetGameplaySettingsAsync(userId);
            return Ok(prefs);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] GameplaySettingsDto dto)
        {
            var userId = User.Identity!.Name!;
            var settings = new GameplaySettings
            {
                VisibleActionBars = dto.VisibleActionBars,
                ShowCooldownNumbers = dto.ShowCooldownNumbers,
                ShowKeybindLabels = dto.ShowKeybindLabels,
                LockActionBars = dto.LockActionBars,

                ShowDamageNumbers = dto.ShowDamageNumbers,
                ShowHealingNumbers = dto.ShowHealingNumbers,
                ShowCriticalEffects = dto.ShowCriticalEffects,
                ShowFloatingCombatText = dto.ShowFloatingCombatText,

                InvertYAxis = dto.InvertYAxis,
                InvertXAxis = dto.InvertXAxis,
                CameraSensitivity = dto.CameraSensitivity,
                FieldOfView = dto.FieldOfView,

                AutoLoot = dto.AutoLoot,
                HighlightInteractables = dto.HighlightInteractables,

                ShowNameplates = dto.ShowNameplates,
                ShowEnemyNameplates = dto.ShowEnemyNameplates,
                ShowFriendlyNameplates = dto.ShowFriendlyNameplates,
                ShowPingIndicators = dto.ShowPingIndicators,

                AutoRunToggle = dto.AutoRunToggle,
                ToggleSprint = dto.ToggleSprint
            };

            await _repo.UpdateGameplaySettingsAsync(userId, settings);
            return Ok();
        }
    }
}
