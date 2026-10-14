using AutoMapper;
using FanModelDB;
using FanStationWifi.WebApi.DTOs;

namespace FanStationWifi.WebApi.Profiles;

public class SensorProfile : AutoMapper.Profile
{
    public SensorProfile()
    {
        CreateMap<Sensor, SensorDTO>();
        CreateMap<SensorDTO, Sensor>();
    }
}