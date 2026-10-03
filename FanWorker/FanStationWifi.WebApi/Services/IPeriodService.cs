using FanStationWifi.WebApi.DTOs;

namespace FanStationWifi.WebApi.Services;

public interface IPeriodService
{
    Task<IEnumerable<PeriodDTO>> GetAllPeriodsByProfileIdAsync(int profileId);
    Task<PeriodDTO?> GetPeriodByIdAsync(int id);
    Task<PeriodDTO?> CreatePeriodAsync(PeriodCreateDTO dto);
    Task<PeriodDTO?> UpdatePeriodAsync(int id, PeriodCreateDTO dto);
    Task<bool> DeletePeriodAsync(int id);
}