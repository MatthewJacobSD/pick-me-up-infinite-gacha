using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickMeUp.Api.Common.Authentication;

namespace PickMeUp.Api.Account.AccountPreferences.Notifications
{
    /// <summary>
    /// Provides full-replace read/write for the notification preferences section of an account.
    /// </summary>
    [ApiController]
    [Route("account/preferences/notifications")]
    [Authorize]
    public sealed class NotificationController(IAccountPreferencesRepository repo, ICurrentUser currentUser) : ControllerBase
    {
        private readonly IAccountPreferencesRepository _repo = repo;
        private readonly ICurrentUser _currentUser = currentUser;

        /**--------[Endpoints]--------**/

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
            return Ok(new { doc.Notifications, doc.Version });
        }

        /// <summary>
        /// Replaces the entire notification settings block, guarded by optimistic-concurrency version check.
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] NotificationDto dto)
        {
            var newVersion = await _repo.ReplaceNotificationSettingsAsync(_currentUser.AccountId, dto.ToSettings(), dto.Version);
            var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
            return Ok(new { doc.Notifications, Version = newVersion });
        }
    }
}
