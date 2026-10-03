using FanStationWifi.WebApi.DTOs;

namespace FanStationWifi.WebApi.Services;

public interface ISettingsService
{
    Task<SettingsDTO?> GetSettingsByProfileIdAsync(int profileId);
    Task<SettingsDTO?> CreateSettingsAsync(SettingsCreateDTO dto);
    Task<SettingsDTO?> UpdateSettingsByProfileIdAsync(int profileId, SettingsCreateDTO dto);
}