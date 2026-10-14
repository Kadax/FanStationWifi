using AutoMapper;
using FanStationWifi.WebApi.DTOs;

namespace FanStationWifi.WebApi.Profiles;

public class ProfileProfile : AutoMapper.Profile
{
    public ProfileProfile()
    {
        CreateMap<FanModelDB.Profile, ProfileDTO>();
        CreateMap<ProfileDTO, FanModelDB.Profile>();
    }
}