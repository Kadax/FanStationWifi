using Microsoft.AspNetCore.Mvc;
using FanStationWifi.WebApi.Services;
using FanStationWifi.WebApi.DTOs;

namespace FanStationWifi.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(401)]
[ProducesResponseType(403)]
[ProducesResponseType(404)]
public class HistoryController : ControllerBase
{
    private readonly ISensorHistoryService _sensorHistoryService;

    public HistoryController(ISensorHistoryService sensorHistoryService)
    {
        _sensorHistoryService = sensorHistoryService;
    }

    [HttpGet("{sensorId}/history")]
    [ProducesResponseType(typeof(IEnumerable<SensorHistoryDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<SensorHistoryDTO>>> GetSensorHistory(int sensorId)
    {
        var history = await _sensorHistoryService.GetSensorHistoryAsync(sensorId);
        return Ok(history);
    }

    [HttpGet("{sensorId}/history/range")]
    [ProducesResponseType(typeof(IEnumerable<SensorHistoryDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<SensorHistoryDTO>>> GetSensorHistoryRange(
        int sensorId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        var history = await _sensorHistoryService.GetSensorHistoryRangeAsync(sensorId, startDate, endDate);
        return Ok(history);
    }
}