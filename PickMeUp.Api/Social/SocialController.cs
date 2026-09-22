using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickMeUp.Api.Common.Authentication;

namespace PickMeUp.Api.Social;

[ApiController]
[Route("account/social")]
[Authorize]
public sealed class SocialController(ISocialService social, ICurrentUser currentUser) : ControllerBase
{
    private readonly ISocialService _social = social;
    private readonly ICurrentUser _currentUser = currentUser;

    [HttpGet("friends")]
    public async Task<IActionResult> GetFriends()
        => Ok(await _social.GetFriendsAsync(RequireAccountId()));

    [HttpDelete("friends/{targetUserId}")]
    public async Task<IActionResult> RemoveFriend(string targetUserId)
    {
        await _social.RemoveFriendAsync(RequireAccountId(), targetUserId);
        return NoContent();
    }

    [HttpPost("friends/requests")]
    public async Task<IActionResult> SendFriendRequest([FromBody] TargetAccountRequest request)
    {
        await _social.SendFriendRequestAsync(RequireAccountId(), request.TargetUserId);
        return Ok();
    }

    [HttpPost("friends/requests/{senderId}/accept")]
    public async Task<IActionResult> AcceptFriendRequest(string senderId)
    {
        await _social.AcceptFriendRequestAsync(RequireAccountId(), senderId);
        return Ok();
    }

    [HttpPost("friends/requests/{senderId}/decline")]
    public async Task<IActionResult> DeclineFriendRequest(string senderId)
    {
        await _social.DeclineFriendRequestAsync(RequireAccountId(), senderId);
        return Ok();
    }

    [HttpDelete("friends/requests/{receiverId}")]
    public async Task<IActionResult> CancelFriendRequest(string receiverId)
    {
        await _social.CancelFriendRequestAsync(RequireAccountId(), receiverId);
        return NoContent();
    }

    [HttpGet("friends/requests")]
    public async Task<IActionResult> ListFriendRequests()
        => Ok((await _social.ListPendingFriendRequestsAsync(RequireAccountId())).Select(ToResponse));

    [HttpGet("blocks")]
    public async Task<IActionResult> GetBlocks()
        => Ok(await _social.GetBlocksAsync(RequireAccountId()));

    [HttpPost("blocks")]
    public async Task<IActionResult> Block([FromBody] TargetAccountRequest request)
    {
        await _social.BlockUserAsync(RequireAccountId(), request.TargetUserId);
        return Ok();
    }

    [HttpDelete("blocks/{targetUserId}")]
    public async Task<IActionResult> Unblock(string targetUserId)
    {
        await _social.UnblockUserAsync(RequireAccountId(), targetUserId);
        return NoContent();
    }

    [HttpPost("party/invites")]
    public async Task<IActionResult> SendPartyInvite([FromBody] TargetAccountRequest request)
    {
        await _social.SendPartyInviteAsync(RequireAccountId(), request.TargetUserId);
        return Ok();
    }

    [HttpPost("party/invites/{senderId}/accept")]
    public async Task<IActionResult> AcceptPartyInvite(string senderId)
    {
        await _social.AcceptPartyInviteAsync(RequireAccountId(), senderId);
        return Ok();
    }

    [HttpPost("party/invites/{senderId}/decline")]
    public async Task<IActionResult> DeclinePartyInvite(string senderId)
    {
        await _social.DeclinePartyInviteAsync(RequireAccountId(), senderId);
        return Ok();
    }

    [HttpDelete("party/invites/{receiverId}")]
    public async Task<IActionResult> CancelPartyInvite(string receiverId)
    {
        await _social.CancelPartyInviteAsync(RequireAccountId(), receiverId);
        return NoContent();
    }

    [HttpGet("party/invites")]
    public async Task<IActionResult> ListPartyInvites()
        => Ok((await _social.ListPendingPartyInvitesAsync(RequireAccountId())).Select(ToResponse));

    private string RequireAccountId()
    {
        if (!_currentUser.IsAuthenticated)
            throw new SocialUnauthenticatedException();

        return _currentUser.AccountId.ToString("D");
    }

    private static FriendRequestResponse ToResponse(FriendRequestDocument request) => new(
        request.Id,
        request.SenderId,
        request.ReceiverId,
        request.Status.ToString(),
        request.CreatedAt,
        request.RespondedAt,
        request.ExpiresAt);

    private static PartyInviteResponse ToResponse(PartyInviteDocument invite) => new(
        invite.Id,
        invite.SenderId,
        invite.ReceiverId,
        invite.Status.ToString(),
        invite.CreatedAt,
        invite.RespondedAt,
        invite.ExpiresAt);
}

public sealed class TargetAccountRequest
{
    public string TargetUserId { get; init; } = string.Empty;
}

public sealed record FriendRequestResponse(
    Guid Id,
    string SenderId,
    string ReceiverId,
    string Status,
    DateTime CreatedAt,
    DateTime? RespondedAt,
    DateTime? ExpiresAt);

public sealed record PartyInviteResponse(
    Guid Id,
    string SenderId,
    string ReceiverId,
    string Status,
    DateTime CreatedAt,
    DateTime? RespondedAt,
    DateTime? ExpiresAt);
