using AutoMapper;
using FanStationWifi.WebApi.Profiles;

namespace FanStationWifi.WebApi.Extensions;

public static class AutoMapperExtensions
{
    public static void ConfigureMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<SensorProfile>();
            cfg.AddProfile<SensorTypeProfile>();
            cfg.AddProfile<SettingsProfile>();
            cfg.AddProfile<ProfileProfile>();
            cfg.AddProfile<PeriodProfile>();
            cfg.AddProfile<SensorHistoryProfile>();
        });
    }
}