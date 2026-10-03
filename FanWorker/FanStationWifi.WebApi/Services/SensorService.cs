using AutoMapper;
using FanModelDB;
using FanStationWifi.WebApi.DTOs;
using FanStationWifi.WebApi.Services;
using Microsoft.EntityFrameworkCore;

namespace FanStationWifi.WebApi.Services;

public class SensorService : ISensorService
{
    private readonly FanModelDBContext _context;
    private readonly IMapper _mapper;

    public SensorService(FanModelDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SensorDTO>> GetAllSensorsAsync()
    {
        var sensors = await _context.Sensors
            .Include(s => s.Type)
            .ToListAsync();

        return sensors.Select(s => _mapper.Map<SensorDTO>(s));
    }

    public async Task<SensorDTO?> GetSensorByIdAsync(int id)
    {
        var sensor = await _context.Sensors
            .Include(s => s.Type)
            .FirstOrDefaultAsync(s => s.Id == id);

        return sensor == null ? null : _mapper.Map<SensorDTO>(sensor);
    }

    public async Task<SensorDTO?> CreateSensorAsync(SensorCreateDTO dto)
    {
        var sensor = new Sensor
        {
            Name = dto.Name,
            TypeId = dto.TypeId,
            IsActive = dto.IsActive,
            Order = dto.Order,
            Source = dto.Source,
            SensorExtID = dto.SensorExtID
        };

        _context.Sensors.Add(sensor);
        await _context.SaveChangesAsync();

        return _mapper.Map<SensorDTO>(sensor);
    }

    public async Task<SensorDTO?> UpdateSensorAsync(int id, SensorCreateDTO dto)
    {
        var sensor = await _context.Sensors.FirstOrDefaultAsync(s => s.Id == id);

        if (sensor == null)
            return null;

        sensor.Name = dto.Name;
        sensor.TypeId = dto.TypeId;
        sensor.IsActive = dto.IsActive;
        sensor.Order = dto.Order;
        sensor.Source = dto.Source;
        sensor.SensorExtID = dto.SensorExtID;

        await _context.SaveChangesAsync();

        return _mapper.Map<SensorDTO>(sensor);
    }

    public async Task<bool> DeleteSensorAsync(int id)
    {
        var sensor = await _context.Sensors.FirstOrDefaultAsync(s => s.Id == id);

        if (sensor == null)
            return false;

        _context.Sensors.Remove(sensor);
        await _context.SaveChangesAsync();

        return true;
    }
}