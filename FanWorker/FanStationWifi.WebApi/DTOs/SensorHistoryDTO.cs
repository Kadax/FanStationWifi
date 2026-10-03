using System.Text.Json.Serialization;

namespace FanStationWifi.WebApi.DTOs;

public class SensorHistoryDTO
{
    public int Id { get; set; }
    public int SensorId { get; set; }
    public DateTime Date { get; set; }
    public double Value { get; set; }
}