using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickMeUp.Api.Account.AccountPreferences;
using PickMeUp.Api.Common.Authentication;

namespace PickMeUp.Api.Account.AccountPreferences.Language
{
    [ApiController]
    [Route("account/preferences/language")]
    [Authorize]
    public sealed class LanguageController : ControllerBase
    {
        private readonly IAccountPreferencesRepository _repo;
        private readonly ICurrentUser _currentUser;

        public LanguageController(IAccountPreferencesRepository repo, ICurrentUser currentUser)
        {
            _repo = repo;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
            return Ok(new { doc.Language, doc.Version });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] LanguageSettingsDto dto)
        {
            var newVersion = await _repo.ReplaceLanguageSettingsAsync(_currentUser.AccountId, dto.ToSettings(), dto.Version);
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
            return Ok(new { doc.Language, Version = newVersion });
        }
    }
}
