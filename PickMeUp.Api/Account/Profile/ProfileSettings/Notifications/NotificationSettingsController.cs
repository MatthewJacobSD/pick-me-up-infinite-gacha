using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PickMeUp.Api.Account.Profile.ProfileSettings.Notifications
{
    [ApiController]
    [Route("account/preferences/notifications")]
    [Authorize]
    public sealed class NotificationSettingsController : ControllerBase
    {
        private readonly IAccountPreferencesRepository _repo;

        public NotificationSettingsController(IAccountPreferencesRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = User.Identity!.Name!;
            var prefs = await _repo.GetNotificationSettingsAsync(userId);
            return Ok(prefs);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] NotificationSettingsDto dto)
        {
            var userId = User.Identity!.Name!;
            var settings = new NotificationSettings
            {
                EventNotifications = dto.EventNotifications,
                FriendRequestNotifications = dto.FriendRequestNotifications,
                PartyInviteNotifications = dto.PartyInviteNotifications,
                SystemAnnouncements = dto.SystemAnnouncements,
                RewardNotifications = dto.RewardNotifications
            };

            await _repo.UpdateNotificationSettingsAsync(userId, settings);
            return Ok();
        }
    }
}
