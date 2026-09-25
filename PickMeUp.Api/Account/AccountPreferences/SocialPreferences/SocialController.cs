using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickMeUp.Api.Account.AccountPreferences;
using PickMeUp.Api.Common.Authentication;

namespace PickMeUp.Api.Account.AccountPreferences.SocialPreferences
{
    [ApiController]
    [Route("account/preferences/social")]
    [Authorize]
    public sealed class SocialController : ControllerBase
    {
        private readonly IAccountPreferencesRepository _repo;
        private readonly ICurrentUser _currentUser;

        public SocialController(IAccountPreferencesRepository repo, ICurrentUser currentUser)
        {
            _repo = repo;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
            return Ok(new { doc.SocialPreferences, doc.Version });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SocialDto dto)
        {
            var newVersion = await _repo.ReplaceSocialPreferencesSettingsAsync(_currentUser.AccountId, dto.ToSettings(), dto.Version);
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
            return Ok(new { doc.SocialPreferences, Version = newVersion });
        }
    }
}
