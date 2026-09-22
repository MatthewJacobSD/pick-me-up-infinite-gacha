using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PickMeUp.Api.Account.AccountPreferences.UiPreferences
{
    [ApiController]
    [Route("account/preferences/ui")]
    [Authorize]
    public sealed class UiPreferencesController : ControllerBase
    {
        private readonly IAccountPreferencesRepository _repo;

        public UiPreferencesController(IAccountPreferencesRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = User.Identity!.Name!;
            var prefs = await _repo.GetUiPreferencesAsync(userId);
            return Ok(prefs);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UiPreferencesDto dto)
        {
            var userId = User.Identity!.Name!;
            var settings = new UiPreferencesSettings
            {
                UiScale = dto.UiScale,
                TextSize = dto.TextSize,
                IconSize = dto.IconSize,

                ShowMinimap = dto.ShowMinimap,
                ShowChatWindow = dto.ShowChatWindow,
                ShowQuestTracker = dto.ShowQuestTracker,
                ShowActionBars = dto.ShowActionBars,

                MinimapPosition = dto.MinimapPosition,
                ChatWindowPosition = dto.ChatWindowPosition,
                QuestTrackerPosition = dto.QuestTrackerPosition,
                ActionBarLayout = dto.ActionBarLayout,

                InventoryLayout = dto.InventoryLayout,
                ChatLayout = dto.ChatLayout
            };

            await _repo.UpdateUiPreferencesAsync(userId, settings);
            return Ok();
        }
    }
}
