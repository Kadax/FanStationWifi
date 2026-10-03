using System.Text.Json.Serialization;

namespace FanStationWifi.WebApi.DTOs;

public class SensorCreateDTO
{
    public required string Name { get; set; }
    public required int TypeId { get; set; }
    public bool IsActive { get; set; } = true;
    public int? Order { get; set; }
    public string? Source { get; set; }
    public string? SensorExtID { get; set; }
}