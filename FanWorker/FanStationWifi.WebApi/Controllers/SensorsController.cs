using Microsoft.AspNetCore.Mvc;
using FanStationWifi.WebApi.Services;
using FanStationWifi.WebApi.DTOs;

namespace FanStationWifi.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(401)]
[ProducesResponseType(403)]
[ProducesResponseType(404)]
public class SensorsController : ControllerBase
{
    private readonly ISensorService _sensorService;

    public SensorsController(ISensorService sensorService)
    {
        _sensorService = sensorService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SensorDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SensorDTO>>> GetAllSensors()
    {
        return Ok(await _sensorService.GetAllSensorsAsync());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SensorDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SensorDTO>> GetSensorById(int id)
    {
        var sensor = await _sensorService.GetSensorByIdAsync(id);
        return sensor == null ? NotFound() : Ok(sensor);
    }

    [HttpPost]
    [ProducesResponseType(typeof(SensorDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SensorDTO>> CreateSensor([FromBody] SensorCreateDTO dto)
    {
        var sensor = await _sensorService.CreateSensorAsync(dto);
        if (sensor == null)
            return BadRequest();

        return CreatedAtAction(nameof(GetSensorById), new { id = sensor.Id }, sensor);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(SensorDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SensorDTO>> UpdateSensor(int id, [FromBody] SensorCreateDTO dto)
    {
        var sensor = await _sensorService.UpdateSensorAsync(id, dto);
        return sensor == null ? NotFound() : Ok(sensor);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSensor(int id)
    {
        var result = await _sensorService.DeleteSensorAsync(id);
        return result ? NoContent() : NotFound();
    }
}