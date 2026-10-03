using AutoMapper;
using FanModelDB;
using FanStationWifi.WebApi.DTOs;
using FanStationWifi.WebApi.Services;
using Microsoft.EntityFrameworkCore;

namespace FanStationWifi.WebApi.Services;

public class SensorTypeService : ISensorTypeService
{
    private readonly FanModelDBContext _context;
    private readonly IMapper _mapper;

    public SensorTypeService(FanModelDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SensorTypeDTO>> GetAllSensorTypesAsync()
    {
        var types = await _context.SensorTypes.ToListAsync();

        return types.Select(t => _mapper.Map<SensorTypeDTO>(t));
    }

    public async Task<SensorTypeDTO?> GetSensorTypeByIdAsync(int id)
    {
        var type = await _context.SensorTypes            
            .FirstOrDefaultAsync(t => t.Id == id);

        return type == null ? null : _mapper.Map<SensorTypeDTO>(type);
    }

    public async Task<SensorTypeDTO?> CreateSensorTypeAsync(SensorTypeCreateDTO dto)
    {
        var type = new SensorType
        {
            Name = dto.Name,
            Description = dto.Description,
            Source = dto.Source
        };

        _context.SensorTypes.Add(type);
        await _context.SaveChangesAsync();

        return _mapper.Map<SensorTypeDTO>(type);
    }

    public async Task<SensorTypeDTO?> UpdateSensorTypeAsync(int id, SensorTypeCreateDTO dto)
    {
        var type = await _context.SensorTypes.FirstOrDefaultAsync(t => t.Id == id);

        if (type == null)
            return null;

        type.Name = dto.Name;
        type.Description = dto.Description;
        type.Source = dto.Source;

        await _context.SaveChangesAsync();

        return _mapper.Map<SensorTypeDTO>(type);
    }

    public async Task<bool> DeleteSensorTypeAsync(int id)
    {
        var type = await _context.SensorTypes.FirstOrDefaultAsync(t => t.Id == id);

        if (type == null)
            return false;

        _context.SensorTypes.Remove(type);
        await _context.SaveChangesAsync();

        return true;
    }
}