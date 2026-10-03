using System.Text.Json.Serialization;

namespace FanStationWifi.WebApi.DTOs;

public class SensorDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int? Order { get; set; }
    public SensorTypeDTO? Type { get; set; }
    public int TypeId { get; set; }
    public string? Source { get; set; }
    public string? SensorExtID { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }
}