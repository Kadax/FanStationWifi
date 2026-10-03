using AutoMapper;
using FanModelDB;
using FanStationWifi.WebApi.DTOs;
using FanStationWifi.WebApi.Services;
using Microsoft.EntityFrameworkCore;

namespace FanStationWifi.WebApi.Services;

public class ProfileService : IProfileService
{
    private readonly FanModelDBContext _context;
    private readonly IMapper _mapper;

    public ProfileService(FanModelDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProfileDTO>> GetAllProfilesAsync()
    {
        var profiles = await _context.Profiles
            .ToListAsync();

        return profiles.Select(p => _mapper.Map<ProfileDTO>(p));
    }

    public async Task<ProfileDTO?> GetProfileByIdAsync(int id)
    {
        var profile = await _context.Profiles
            .FirstOrDefaultAsync(p => p.Id == id);

        return profile == null ? null : _mapper.Map<ProfileDTO>(profile);
    }

    public async Task<ProfileDTO?> CreateProfileAsync(ProfileCreateDTO dto)
    {
        var profile = new FanModelDB.Profile
        {
            Name = dto.Name,
            IsActive = dto.IsActive,
            Order = dto.Order
        };

        _context.Profiles.Add(profile);
        await _context.SaveChangesAsync();

        return _mapper.Map<ProfileDTO>(profile);
    }

    public async Task<ProfileDTO?> UpdateProfileAsync(int id, ProfileCreateDTO dto)
    {
        var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.Id == id);

        if (profile == null)
            return null;

        profile.Name = dto.Name;
        profile.IsActive = dto.IsActive;
        profile.Order = dto.Order;

        await _context.SaveChangesAsync();

        return _mapper.Map<ProfileDTO>(profile);
    }

    public async Task<bool> DeleteProfileAsync(int id)
    {
        var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.Id == id);

        if (profile == null)
            return false;

        _context.Profiles.Remove(profile);
        await _context.SaveChangesAsync();

        return true;
    }
}