using AutoMapper;
using FanModelDB;
using FanStationWifi.WebApi.DTOs;

namespace FanStationWifi.WebApi.Profiles;

public class SettingsProfile : AutoMapper.Profile
{
    public SettingsProfile()
    {
        CreateMap<Settings, SettingsDTO>();
        CreateMap<SettingsDTO, Settings>();
    }
}