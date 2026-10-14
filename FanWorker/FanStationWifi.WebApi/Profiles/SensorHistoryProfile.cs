using AutoMapper;
using FanModelDB;
using FanStationWifi.WebApi.DTOs;

namespace FanStationWifi.WebApi.Profiles;

public class SensorHistoryProfile : AutoMapper.Profile
{
    public SensorHistoryProfile()
    {
        CreateMap<SensorsHistory, SensorHistoryDTO>();
        CreateMap<SensorHistoryDTO, SensorsHistory>();
    }
}