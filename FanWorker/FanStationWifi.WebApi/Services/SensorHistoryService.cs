using AutoMapper;
using FanModelDB;
using FanStationWifi.WebApi.DTOs;
using FanStationWifi.WebApi.Services;
using Microsoft.EntityFrameworkCore;

namespace FanStationWifi.WebApi.Services;

public class SensorHistoryService : ISensorHistoryService
{
    private readonly FanModelDBContext _context;
    private readonly IMapper _mapper;

    public SensorHistoryService(FanModelDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SensorHistoryDTO>> GetSensorHistoryAsync(int sensorId)
    {
        var history = await _context.SensorsHistory
            .Where(h => h.SensorId == sensorId)
            .OrderByDescending(h => h.Date)
            .ToListAsync();

        return history.Select(h => _mapper.Map<SensorHistoryDTO>(h));
    }

    public async Task<IEnumerable<SensorHistoryDTO>> GetSensorHistoryRangeAsync(int sensorId, DateTime startDate, DateTime endDate)
    {
        var history = await _context.SensorsHistory
            .Where(h => h.SensorId == sensorId && h.Date >= startDate && h.Date <= endDate)
            .OrderByDescending(h => h.Date)
            .ToListAsync();

        return history.Select(h => _mapper.Map<SensorHistoryDTO>(h));
    }
}