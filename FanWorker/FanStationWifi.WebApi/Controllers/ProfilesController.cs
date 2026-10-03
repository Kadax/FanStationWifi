using Microsoft.AspNetCore.Mvc;
using FanStationWifi.WebApi.Services;
using FanStationWifi.WebApi.DTOs;

namespace FanStationWifi.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(401)]
[ProducesResponseType(403)]
[ProducesResponseType(404)]
public class ProfilesController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfilesController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProfileDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProfileDTO>>> GetAllProfiles()
    {
        return Ok(await _profileService.GetAllProfilesAsync());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProfileDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProfileDTO>> GetProfileById(int id)
    {
        var profile = await _profileService.GetProfileByIdAsync(id);
        return profile == null ? NotFound() : Ok(profile);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProfileDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProfileDTO>> CreateProfile([FromBody] ProfileCreateDTO dto)
    {
        var profile = await _profileService.CreateProfileAsync(dto);
        if (profile == null)
            return BadRequest();

        return CreatedAtAction(nameof(GetProfileById), new { id = profile.Id }, profile);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ProfileDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProfileDTO>> UpdateProfile(int id, [FromBody] ProfileCreateDTO dto)
    {
        var profile = await _profileService.UpdateProfileAsync(id, dto);
        return profile == null ? NotFound() : Ok(profile);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProfile(int id)
    {
        var result = await _profileService.DeleteProfileAsync(id);
        return result ? NoContent() : NotFound();
    }
}