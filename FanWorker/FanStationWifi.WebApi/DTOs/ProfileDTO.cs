using System.Text.Json.Serialization;

namespace FanStationWifi.WebApi.DTOs;

public class ProfileDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int Order { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }
    public SettingsDTO? Settings { get; set; }
    public IEnumerable<PeriodDTO>? Periods { get; set; }
}