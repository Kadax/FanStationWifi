using AutoMapper;
using FanModelDB;
using FanStationWifi.WebApi.DTOs;

namespace FanStationWifi.WebApi.Profiles;

public class PeriodProfile : AutoMapper.Profile
{
    public PeriodProfile()
    {
        CreateMap<Period, PeriodDTO>();
        CreateMap<PeriodDTO, Period>();
    }
}