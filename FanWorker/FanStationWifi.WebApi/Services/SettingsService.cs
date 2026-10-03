using AutoMapper;
using FanModelDB;
using FanStationWifi.WebApi.DTOs;
using FanStationWifi.WebApi.Services;
using Microsoft.EntityFrameworkCore;

namespace FanStationWifi.WebApi.Services;

public class SettingsService : ISettingsService
{
    private readonly FanModelDBContext _context;
    private readonly IMapper _mapper;

    public SettingsService(FanModelDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SettingsDTO?> GetSettingsByProfileIdAsync(int profileId)
    {
        var settings = await _context.Settings
            .FirstOrDefaultAsync(s => s.ProfileId == profileId);

        return settings == null ? null : _mapper.Map<SettingsDTO>(settings);
    }

    public async Task<SettingsDTO?> CreateSettingsAsync(SettingsCreateDTO dto)
    {
        var settings = new Settings
        {
            Name = dto.Name,
            GivenTemperature = dto.GivenTemperature,
            MaxSpeed = dto.MaxSpeed,
            PeriodUpdete = dto.PeriodUpdete,
            isAuto = dto.isAuto,
            minDelta = dto.minDelta,
            maxDelta = dto.maxDelta,
            maxDeltaExternal = dto.maxDeltaExternal,
            minExternal = dto.minExternal,
            topDelteRoom = dto.topDelteRoom,
            minRoom = dto.minRoom,
            ProfileId = dto.ProfileId
        };

        _context.Settings.Add(settings);
        await _context.SaveChangesAsync();

        return _mapper.Map<SettingsDTO>(settings);
    }

    public async Task<SettingsDTO?> UpdateSettingsByProfileIdAsync(int profileId, SettingsCreateDTO dto)
    {
        var settings = await _context.Settings.FirstOrDefaultAsync(s => s.ProfileId == profileId);

        if (settings == null)
            return null;

        settings.Name = dto.Name;
        settings.GivenTemperature = dto.GivenTemperature;
        settings.MaxSpeed = dto.MaxSpeed;
        settings.PeriodUpdete = dto.PeriodUpdete;
        settings.isAuto = dto.isAuto;
        settings.minDelta = dto.minDelta;
        settings.maxDelta = dto.maxDelta;
        settings.maxDeltaExternal = dto.maxDeltaExternal;
        settings.minExternal = dto.minExternal;
        settings.topDelteRoom = dto.topDelteRoom;
        settings.minRoom = dto.minRoom;

        await _context.SaveChangesAsync();

        return _mapper.Map<SettingsDTO>(settings);
    }
}