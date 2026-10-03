using AutoMapper;
using FanModelDB;
using FanStationWifi.WebApi.DTOs;

namespace FanStationWifi.WebApi.Extensions;

public static class AutoMapperExtensions
{
    public static void ConfigureMappings(this IServiceCollection services)
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Profile, ProfileDTO>();
            cfg.CreateMap<ProfileCreateDTO, Profile>();

            cfg.CreateMap<Settings, SettingsDTO>();
            cfg.CreateMap<SettingsCreateDTO, Settings>();

            cfg.CreateMap<Sensor, SensorDTO>();
            cfg.CreateMap<SensorCreateDTO, Sensor>();

            cfg.CreateMap<SensorType, SensorTypeDTO>();
            cfg.CreateMap<SensorTypeCreateDTO, SensorType>();

            cfg.CreateMap<Period, PeriodDTO>();
            cfg.CreateMap<PeriodCreateDTO, Period>();

            cfg.CreateMap<SensorsHistory, SensorHistoryDTO>();
        });

        services.AddSingleton(config.CreateMapper());
    }
}