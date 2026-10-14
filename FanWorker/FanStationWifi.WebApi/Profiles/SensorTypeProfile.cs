using AutoMapper;
using FanModelDB;
using FanStationWifi.WebApi.DTOs;

namespace FanStationWifi.WebApi.Profiles;

public class SensorTypeProfile : AutoMapper.Profile
{
    public SensorTypeProfile()
    {
        CreateMap<SensorType, SensorTypeDTO>();
        CreateMap<SensorTypeDTO, SensorType>();
    }
}