using System.Text.Json.Serialization;

namespace FanStationWifi.WebApi.DTOs;

public class SensorTypeCreateDTO
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Source { get; set; }
}