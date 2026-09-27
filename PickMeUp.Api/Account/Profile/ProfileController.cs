using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickMeUp.Api.Common.Authentication;

namespace PickMeUp.Api.Account.Profile;

/**--------[Controller]--------**/

/// <summary>
/// REST endpoints for reading and updating the authenticated player's profile.
/// </summary>
[ApiController]
[Route("account/profile")]
[Authorize]
public sealed class ProfileController : ControllerBase
{
    private readonly IProfileRepository _repo;
    private readonly ICurrentUser _currentUser;

    public ProfileController(IProfileRepository repo, ICurrentUser currentUser)
    {
        _repo = repo;
        _currentUser = currentUser;
    }

    // GET /account/profile — returns the caller's profile (auto-created on first hit).
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
        return Ok(ProfileDto.FromDocument(doc));
    }

    // PUT /account/profile/username — replaces the caller's display name.
    // Version is required for optimistic concurrency; client must send the version it last read.
    [HttpPut("username")]
    public async Task<IActionResult> UpdateUsername([FromBody] UpdateUsernameDto dto)
    {
        var username = Username.Create(dto.Username);
        var doc = await _repo.UpdateUsernameAsync(_currentUser.AccountId, username, dto.Version);
        return Ok(ProfileDto.FromDocument(doc));
    }

    // PUT /account/profile/avatar — replaces the caller's avatar selection.
    [HttpPut("avatar")]
    public async Task<IActionResult> UpdateAvatar([FromBody] UpdateAvatarDto dto)
    {
        var avatar = Avatar.Create(dto.Value, dto.AvatarUrlPath, dto.AvatarType);
        var doc = await _repo.UpdateAvatarAsync(_currentUser.AccountId, avatar, dto.Version);
        return Ok(ProfileDto.FromDocument(doc));
    }
}
