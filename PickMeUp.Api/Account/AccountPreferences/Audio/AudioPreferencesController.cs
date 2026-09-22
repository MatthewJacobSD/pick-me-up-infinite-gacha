using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickMeUp.Api.Account.AccountPreferences;
using PickMeUp.Api.Common.Authentication;

namespace PickMeUp.Api.Account.AccountPreferences.Audio
{
    [ApiController]
    [Route("account/preferences/audio")]
    [Authorize]
    public sealed class AudioPreferencesController : ControllerBase
    {
        private readonly IAccountPreferencesRepository _repo;
        private readonly ICurrentUser _currentUser;

        public AudioPreferencesController(IAccountPreferencesRepository repo, ICurrentUser currentUser)
        {
            _repo = repo;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
            return Ok(new { doc.Audio, doc.Version });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AudioPreferencesDto dto)
        {
            var newVersion = await _repo.UpdateAudioPreferencesAsync(_currentUser.AccountId, dto.ToSettings(), dto.Version);
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
            return Ok(new { doc.Audio, Version = newVersion });
        }
    }
}
