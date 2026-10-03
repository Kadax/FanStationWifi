using FanStationWifi.WebApi.DTOs;

namespace FanStationWifi.WebApi.Services;

public interface ISensorTypeService
{
    Task<IEnumerable<SensorTypeDTO>> GetAllSensorTypesAsync();
    Task<SensorTypeDTO?> GetSensorTypeByIdAsync(int id);
    Task<SensorTypeDTO?> CreateSensorTypeAsync(SensorTypeCreateDTO dto);
    Task<SensorTypeDTO?> UpdateSensorTypeAsync(int id, SensorTypeCreateDTO dto);
    Task<bool> DeleteSensorTypeAsync(int id);
}