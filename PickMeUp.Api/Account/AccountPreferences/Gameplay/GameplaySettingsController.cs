using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickMeUp.Api.Account.AccountPreferences;
using PickMeUp.Api.Common.Authentication;

namespace PickMeUp.Api.Account.AccountPreferences.Gameplay
{
    [ApiController]
    [Route("account/preferences/gameplay")]
    [Authorize]
    public sealed class GameplaySettingsController : ControllerBase
    {
        private readonly IAccountPreferencesRepository _repo;
        private readonly ICurrentUser _currentUser;

        public GameplaySettingsController(IAccountPreferencesRepository repo, ICurrentUser currentUser)
        {
            _repo = repo;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
            return Ok(new { doc.Gameplay, doc.Version });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] GameplaySettingsDto dto)
        {
            var newVersion = await _repo.UpdateGameplaySettingsAsync(_currentUser.AccountId, dto.ToSettings(), dto.Version);
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
            return Ok(new { doc.Gameplay, Version = newVersion });
        }
    }
}
