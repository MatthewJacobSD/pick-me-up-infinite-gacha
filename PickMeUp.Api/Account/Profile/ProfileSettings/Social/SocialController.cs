using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PickMeUp.Api.Account.Profile.ProfileSettings.Social
{
    // REST controller for social operations.
    //
    // Endpoints:
    //   GET    /account/social/friends           — list friends
    //   POST   /account/social/friends/requests  — send friend request
    //   DELETE /account/social/friends/{id}      — remove friend
    //   GET    /account/social/blocks            — list blocks
    //   POST   /account/social/blocks            — block user
    //   DELETE /account/social/blocks/{id}       — unblock user
    //   POST   /account/social/party             — party invite

    [ApiController]
    [Route("account/social")]
    [Authorize]
    public sealed class SocialController : ControllerBase
    {
        private readonly ISocialService _socialService;

        public SocialController(ISocialService socialService)
        {
            _socialService = socialService;
        }

        // ── Friends ───────────────────────────────────────────────

        [HttpGet("friends")]
        public async Task<IActionResult> GetFriends()
        {
            var userId = User.Identity!.Name!;
            var friends = await _socialService.GetFriendsAsync(userId);
            return Ok(friends);
        }

        [HttpPost("friends/requests")]
        public async Task<IActionResult> SendFriendRequest([FromBody] FriendCommand command)
        {
            var userId = User.Identity!.Name!;
            await _socialService.SendFriendRequestAsync(userId, command);
            return Ok();
        }

        [HttpDelete("friends/{targetUserId}")]
        public async Task<IActionResult> RemoveFriend(string targetUserId)
        {
            var userId = User.Identity!.Name!;
            await _socialService.RemoveFriendAsync(userId, targetUserId);
            return Ok();
        }

        // ── Blocks ────────────────────────────────────────────────

        [HttpGet("blocks")]
        public async Task<IActionResult> GetBlocks()
        {
            var userId = User.Identity!.Name!;
            var blocks = await _socialService.GetBlocksAsync(userId);
            return Ok(blocks);
        }

        [HttpPost("blocks")]
        public async Task<IActionResult> BlockUser([FromBody] BlockCommand command)
        {
            var userId = User.Identity!.Name!;
            await _socialService.BlockUserAsync(userId, command);
            return Ok();
        }

        [HttpDelete("blocks/{targetUserId}")]
        public async Task<IActionResult> UnblockUser(string targetUserId)
        {
            var userId = User.Identity!.Name!;
            await _socialService.UnblockUserAsync(userId, targetUserId);
            return Ok();
        }

        // ── Party ─────────────────────────────────────────────────

        [HttpPost("party")]
        public async Task<IActionResult> PartyInvite([FromBody] PartyCommand command)
        {
            var userId = User.Identity!.Name!;
            await _socialService.HandlePartyInviteAsync(userId, command);
            return Ok();
        }
    }
}
