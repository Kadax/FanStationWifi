using Microsoft.AspNetCore.Mvc;
using FanStationWifi.WebApi.Services;
using FanStationWifi.WebApi.DTOs;

namespace FanStationWifi.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(401)]
[ProducesResponseType(403)]
[ProducesResponseType(404)]
public class SettingsController : ControllerBase
{
    private readonly ISettingsService _settingsService;

    public SettingsController(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    [HttpGet("{profileId}")]
    [ProducesResponseType(typeof(SettingsDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SettingsDTO>> GetSettingsByProfileId(int profileId)
    {
        var settings = await _settingsService.GetSettingsByProfileIdAsync(profileId);
        return settings == null ? NotFound() : Ok(settings);
    }

    [HttpPost("{profileId}")]
    [ProducesResponseType(typeof(SettingsDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SettingsDTO>> CreateSettings(int profileId, [FromBody] SettingsCreateDTO dto)
    {
        dto.ProfileId = profileId;
        var settings = await _settingsService.CreateSettingsAsync(dto);
        if (settings == null)
            return BadRequest();

        return CreatedAtAction(nameof(GetSettingsByProfileId), new { profileId }, settings);
    }

    [HttpPut("{profileId}")]
    [ProducesResponseType(typeof(SettingsDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SettingsDTO>> UpdateSettings(int profileId, [FromBody] SettingsCreateDTO dto)
    {
        var settings = await _settingsService.UpdateSettingsByProfileIdAsync(profileId, dto);
        return settings == null ? NotFound() : Ok(settings);
    }
}