using System.Text.Json.Serialization;

namespace FanStationWifi.WebApi.DTOs;

public class SettingsDTO
{
    public int Id { get; set; }
    public int ProfileId { get; set; }
    public string Name { get; set; } = string.Empty;
    public double GivenTemperature { get; set; }
    public int MaxSpeed { get; set; }
    public int PeriodUpdete { get; set; }
    public bool isAuto { get; set; }
    public double? minDelta { get; set; }
    public double? maxDelta { get; set; }
    public double? maxDeltaExternal { get; set; }
    public double? minExternal { get; set; }
    public double? topDelteRoom { get; set; }
    public double? minRoom { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }
}