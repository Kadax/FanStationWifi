using FanStationWifi.WebApi.DTOs;

namespace FanStationWifi.WebApi.Services;

public interface ISensorService
{
    Task<IEnumerable<SensorDTO>> GetAllSensorsAsync();
    Task<SensorDTO?> GetSensorByIdAsync(int id);
    Task<SensorDTO?> CreateSensorAsync(SensorCreateDTO dto);
    Task<SensorDTO?> UpdateSensorAsync(int id, SensorCreateDTO dto);
    Task<bool> DeleteSensorAsync(int id);
}