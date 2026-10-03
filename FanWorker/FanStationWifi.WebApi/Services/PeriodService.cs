using AutoMapper;
using FanModelDB;
using FanStationWifi.WebApi.DTOs;
using FanStationWifi.WebApi.Services;
using Microsoft.EntityFrameworkCore;

namespace FanStationWifi.WebApi.Services;

public class PeriodService : IPeriodService
{
    private readonly FanModelDBContext _context;
    private readonly IMapper _mapper;

    public PeriodService(FanModelDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PeriodDTO>> GetAllPeriodsByProfileIdAsync(int profileId)
    {
        var periods = await _context.Periods
            .Where(p => p.ProfileId == profileId)
            .ToListAsync();

        return periods.Select(p => _mapper.Map<PeriodDTO>(p));
    }

    public async Task<PeriodDTO?> GetPeriodByIdAsync(int id)
    {
        var period = await _context.Periods
            .FirstOrDefaultAsync(p => p.Id == id);

        return period == null ? null : _mapper.Map<PeriodDTO>(period);
    }

    public async Task<PeriodDTO?> CreatePeriodAsync(PeriodCreateDTO dto)
    {
        var period = new Period
        {
            Name = dto.Name,
            MaxSpeed = dto.MaxSpeed,
            Start = TimeSpan.FromHours(dto.StartHour).Add(TimeSpan.FromMinutes(dto.StartMinute)),
            End = TimeSpan.FromHours(dto.EndHour).Add(TimeSpan.FromMinutes(dto.EndMinute)),
            ProfileId = dto.ProfileId
        };

        _context.Periods.Add(period);
        await _context.SaveChangesAsync();

        return _mapper.Map<PeriodDTO>(period);
    }

    public async Task<PeriodDTO?> UpdatePeriodAsync(int id, PeriodCreateDTO dto)
    {
        var period = await _context.Periods.FirstOrDefaultAsync(p => p.Id == id);

        if (period == null)
            return null;

        period.Name = dto.Name;
        period.MaxSpeed = dto.MaxSpeed;
        period.Start = TimeSpan.FromHours(dto.StartHour).Add(TimeSpan.FromMinutes(dto.StartMinute));
        period.End = TimeSpan.FromHours(dto.EndHour).Add(TimeSpan.FromMinutes(dto.EndMinute));

        await _context.SaveChangesAsync();

        return _mapper.Map<PeriodDTO>(period);
    }

    public async Task<bool> DeletePeriodAsync(int id)
    {
        var period = await _context.Periods.FirstOrDefaultAsync(p => p.Id == id);

        if (period == null)
            return false;

        _context.Periods.Remove(period);
        await _context.SaveChangesAsync();

        return true;
    }
}