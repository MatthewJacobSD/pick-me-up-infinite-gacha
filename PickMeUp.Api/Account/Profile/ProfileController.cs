using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickMeUp.Api.Common.Authentication;

namespace PickMeUp.Api.Account.Profile;

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

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var doc = await _repo.GetOrCreateAsync(_currentUser.AccountId);
        return Ok(ProfileDto.FromDocument(doc));
    }

    [HttpPut("username")]
    public async Task<IActionResult> UpdateUsername([FromBody] UpdateUsernameDto dto)
    {
        var username = Username.Create(dto.Username);
        var doc = await _repo.UpdateUsernameAsync(_currentUser.AccountId, username, dto.Version);
        return Ok(ProfileDto.FromDocument(doc));
    }

    [HttpPut("avatar")]
    public async Task<IActionResult> UpdateAvatar([FromBody] UpdateAvatarDto dto)
    {
        var avatar = Avatar.Create(dto.Value, dto.AvatarUrlPath, dto.AvatarType);
        var doc = await _repo.UpdateAvatarAsync(_currentUser.AccountId, avatar, dto.Version);
        return Ok(ProfileDto.FromDocument(doc));
    }
}
