using FanModelDB;
using FanStationWifi.WebApi.Extensions;
using FanStationWifi.WebApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddDbContext<FanModelDBContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

// Register services
builder.Services.AddScoped<ISensorService, SensorService>();
builder.Services.AddScoped<ISensorTypeService, SensorTypeService>();
builder.Services.AddScoped<ISettingsService, SettingsService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IPeriodService, PeriodService>();
builder.Services.AddScoped<ISensorHistoryService, SensorHistoryService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FanStation API",
        Version = "v1",
        Description = "REST API для управления системой вентиляции FanStationWifi"
    });

    //c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    //{
    //    Type = SecuritySchemeType.ApiKey,
    //    In = ParameterLocation.Header,
    //    Name = "X-API-KEY",
    //    Description = "Введите API ключ для доступа к API"
    //});
                     
});


builder.Services.ConfigureMappings();

var app = builder.Build();

//app.UseRouting();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FanStation API V1");
    c.RoutePrefix = string.Empty;
});

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FanModelDBContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();

app.Run();