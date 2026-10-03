using System.Text.Json.Serialization;

namespace FanStationWifi.WebApi.DTOs;

public class PeriodCreateDTO
{
    public required string Name { get; set; }
    public int MaxSpeed { get; set; }
    public int StartHour { get; set; }
    public int StartMinute { get; set; }
    public int EndHour { get; set; }
    public int EndMinute { get; set; }
    public int ProfileId { set; get; }
}