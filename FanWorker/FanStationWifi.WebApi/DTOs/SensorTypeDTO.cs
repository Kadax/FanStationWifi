using System.Text.Json.Serialization;

namespace FanStationWifi.WebApi.DTOs;

public class SensorTypeDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Source { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }
    public IEnumerable<SensorDTO>? Sensors { get; set; }
}