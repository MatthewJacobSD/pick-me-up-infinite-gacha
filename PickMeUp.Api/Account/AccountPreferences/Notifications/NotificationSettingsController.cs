using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickMeUp.Api.Account.AccountPreferences;
using PickMeUp.Api.Common.Authentication;

namespace PickMeUp.Api.Account.AccountPreferences.Notifications
{
    [ApiController]
    [Route("account/preferences/notifications")]
    [Authorize]
    public sealed class NotificationSettingsController : ControllerBase
    {
        private readonly IAccountPreferencesRepository _repo;
        private readonly ICurrentUser _currentUser;

        public NotificationSettingsController(IAccountPreferencesRepository repo, ICurrentUser currentUser)
        {
            _repo = repo;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
            return Ok(new { doc.Notifications, doc.Version });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] NotificationSettingsDto dto)
        {
            var newVersion = await _repo.UpdateNotificationSettingsAsync(_currentUser.AccountId, dto.ToSettings(), dto.Version);
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
            return Ok(new { doc.Notifications, Version = newVersion });
        }
    }
}
