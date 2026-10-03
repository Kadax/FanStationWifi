using FanStationWifi.WebApi.DTOs;

namespace FanStationWifi.WebApi.Services;

public interface IProfileService
{
    Task<IEnumerable<ProfileDTO>> GetAllProfilesAsync();
    Task<ProfileDTO?> GetProfileByIdAsync(int id);
    Task<ProfileDTO?> CreateProfileAsync(ProfileCreateDTO dto);
    Task<ProfileDTO?> UpdateProfileAsync(int id, ProfileCreateDTO dto);
    Task<bool> DeleteProfileAsync(int id);
}