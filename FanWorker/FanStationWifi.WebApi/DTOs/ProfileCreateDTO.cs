using System.Text.Json.Serialization;

namespace FanStationWifi.WebApi.DTOs;

public class ProfileCreateDTO
{
    public required string Name { get; set; }
    public bool IsActive { get; set; } = true;
    public int Order { get; set; } = 0;
}