using System.Text.Json.Serialization;

namespace FanStationWifi.WebApi.DTOs;

public class PeriodDTO
{
    public int Id { get; set; }
    public int ProfileId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int MaxSpeed { get; set; }
    public TimeSpan Start { get; set; }
    public TimeSpan End { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }
}