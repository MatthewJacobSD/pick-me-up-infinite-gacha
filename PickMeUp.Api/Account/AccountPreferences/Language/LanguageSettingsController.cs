using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickMeUp.Api.Account.AccountPreferences;

namespace PickMeUp.Api.Account.AccountPreferences.Language
{
    [ApiController]
    [Route("account/preferences/language")]
    [Authorize]
    public sealed class LanguageSettingsController : ControllerBase
    {
        private readonly IAccountPreferencesRepository _repo;

        public LanguageSettingsController(IAccountPreferencesRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = User.Identity!.Name!;
            var prefs = await _repo.GetLanguageSettingsAsync(userId);
            return Ok(prefs);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] LanguageSettingsDto dto)
        {
            var userId = User.Identity!.Name!;
            var settings = new LanguageSettings
            {
                PreferredLanguage = dto.PreferredLanguage
            };

            await _repo.UpdateLanguageSettingsAsync(userId, settings);
            return Ok();
        }
    }
}
