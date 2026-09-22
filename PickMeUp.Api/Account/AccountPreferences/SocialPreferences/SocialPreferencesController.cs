using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PickMeUp.Api.Account.AccountPreferences.SocialPreferences
{
    [ApiController]
    [Route("account/preferences/social")]
    [Authorize]
    public sealed class SocialPreferencesController(IAccountPreferencesRepository repo) : ControllerBase
    {
        private readonly IAccountPreferencesRepository _repo = repo;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = User.Identity!.Name!;
            var prefs = await _repo.GetSocialPreferencesAsync(userId);
            return Ok(prefs);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SocialPreferencesDto dto)
        {
            var userId = User.Identity!.Name!;
            var settings = new SocialPreferencesSettings
            {
                FriendRequests = dto.FriendRequests,
                Messages = dto.Messages,
                PartyInvites = dto.PartyInvites,
                OnlineStatus = dto.OnlineStatus
            };

            await _repo.UpdateSocialPreferencesAsync(userId, settings);
            return Ok();
        }
    }
}
