using FanStationWifi.WebApi.DTOs;

namespace FanStationWifi.WebApi.Services;

public interface ISensorHistoryService
{
    Task<IEnumerable<SensorHistoryDTO>> GetSensorHistoryAsync(int sensorId);
    Task<IEnumerable<SensorHistoryDTO>> GetSensorHistoryRangeAsync(int sensorId, DateTime startDate, DateTime endDate);
}