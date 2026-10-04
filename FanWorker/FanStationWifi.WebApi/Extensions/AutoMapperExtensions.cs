using AutoMapper;
using FanModelDB;
using FanStationWifi.WebApi.DTOs;

namespace FanStationWifi.WebApi.Extensions;

public static class AutoMapperExtensions
{
    public static void ConfigureMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Program));
    }
}