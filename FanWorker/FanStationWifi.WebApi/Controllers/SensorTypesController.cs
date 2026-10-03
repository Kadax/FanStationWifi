using Microsoft.AspNetCore.Mvc;
using FanStationWifi.WebApi.Services;
using FanStationWifi.WebApi.DTOs;

namespace FanStationWifi.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(401)]
[ProducesResponseType(403)]
[ProducesResponseType(404)]
public class SensorTypesController : ControllerBase
{
    private readonly ISensorTypeService _sensorTypeService;

    public SensorTypesController(ISensorTypeService sensorTypeService)
    {
        _sensorTypeService = sensorTypeService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SensorTypeDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SensorTypeDTO>>> GetAllSensorTypes()
    {
        return Ok(await _sensorTypeService.GetAllSensorTypesAsync());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SensorTypeDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SensorTypeDTO>> GetSensorTypeById(int id)
    {
        var sensorType = await _sensorTypeService.GetSensorTypeByIdAsync(id);
        return sensorType == null ? NotFound() : Ok(sensorType);
    }

    [HttpPost]
    [ProducesResponseType(typeof(SensorTypeDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SensorTypeDTO>> CreateSensorType([FromBody] SensorTypeCreateDTO dto)
    {
        var sensorType = await _sensorTypeService.CreateSensorTypeAsync(dto);
        if (sensorType == null)
            return BadRequest();

        return CreatedAtAction(nameof(GetSensorTypeById), new { id = sensorType.Id }, sensorType);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(SensorTypeDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SensorTypeDTO>> UpdateSensorType(int id, [FromBody] SensorTypeCreateDTO dto)
    {
        var sensorType = await _sensorTypeService.UpdateSensorTypeAsync(id, dto);
        return sensorType == null ? NotFound() : Ok(sensorType);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSensorType(int id)
    {
        var result = await _sensorTypeService.DeleteSensorTypeAsync(id);
        return result ? NoContent() : NotFound();
    }
}