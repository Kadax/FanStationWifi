using Microsoft.AspNetCore.Mvc;
using FanStationWifi.WebApi.Services;
using FanStationWifi.WebApi.DTOs;

namespace FanStationWifi.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(401)]
[ProducesResponseType(403)]
[ProducesResponseType(404)]
public class PeriodsController : ControllerBase
{
    private readonly IPeriodService _periodService;

    public PeriodsController(IPeriodService periodService)
    {
        _periodService = periodService;
    }

    [HttpGet("{profileId}")]
    [ProducesResponseType(typeof(IEnumerable<PeriodDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PeriodDTO>>> GetAllPeriodsByProfileId(int profileId)
    {
        return Ok(await _periodService.GetAllPeriodsByProfileIdAsync(profileId));
    }

    [HttpGet("id/{id}")]
    [ProducesResponseType(typeof(PeriodDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PeriodDTO>> GetPeriodById(int id)
    {
        var period = await _periodService.GetPeriodByIdAsync(id);
        return period == null ? NotFound() : Ok(period);
    }

    [HttpPost("{profileId}")]
    [ProducesResponseType(typeof(PeriodDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PeriodDTO>> CreatePeriod(int profileId, [FromBody] PeriodCreateDTO dto)
    {
        dto.ProfileId = profileId;
        var period = await _periodService.CreatePeriodAsync(dto);
        if (period == null)
            return BadRequest();

        return CreatedAtAction(nameof(GetPeriodById), new { id = period.Id }, period);
    }

    [HttpPut("id/{id}")]
    [ProducesResponseType(typeof(PeriodDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PeriodDTO>> UpdatePeriod(int id, [FromBody] PeriodCreateDTO dto)
    {
        var period = await _periodService.UpdatePeriodAsync(id, dto);
        return period == null ? NotFound() : Ok(period);
    }

    [HttpDelete("id/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePeriod(int id)
    {
        var result = await _periodService.DeletePeriodAsync(id);
        return result ? NoContent() : NotFound();
    }
}