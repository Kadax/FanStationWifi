using FanModelDB;
using FanWorker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Runtime.CompilerServices;

Console.WriteLine("Start...");

var db_factory = new FanModelDBContextFactory();

var db =  db_factory.CreateDbContext(null);

await db.Database.MigrateAsync();

Console.WriteLine("Get Config...");

string c = Directory.GetCurrentDirectory();

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);

IConfiguration config = builder.Build();





using var httpClient = new HttpClient
{
    Timeout = TimeSpan.FromSeconds(30)
};

var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("Program");

var prometheusClient = new PrometheusClient(httpClient, logger);

try
{
    var sensors_p = await prometheusClient.GetSensorsAsync(
                    config.GetValue<string>("Prometheus:ServerAddress"), 
                    config.GetValue<int>("Prometheus:MaxRetries"),
                    config.GetValue<int>("Prometheus:TimeoutSeconds")
                  );


    Console.WriteLine($"Found {sensors_p.Count} sensors:");

    foreach (var sensor in sensors_p)
    {
        Console.WriteLine($"  - {sensor}");
    }

    int timeout = 1;

    var types = await db.SensorTypes.ToListAsync();
    
    var esp8266Client = new ESP8266Client(config.GetValue<string>("FanStation:address"), logger);

    while (true)
    {

        logger.Log(LogLevel.Information, $"Step {DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}");




        var p = await db.Profiles.FirstOrDefaultAsync(i => i.IsActive == true);

        if( p != null)
        {
            var settings = await db.Settings.FirstOrDefaultAsync(i => i.ProfileId == p.Id);

            if (settings != null)
            {
                timeout = settings.PeriodUpdete;

                var sensors = db.Sensors.Where(i => i.IsActive).ToList();
                                             
                logger.Log(LogLevel.Information, $"Fetching Fans status");
                var fansStatus = await esp8266Client.GetStatusAsync();

                if (fansStatus != null)
                {
                    logger.Log(LogLevel.Information,
                        $"Fans state - Power: {fansStatus.Power}, Direction: {fansStatus.Direction}, " +
                        $"Speed: {fansStatus.Speed}, ServoPosition: {fansStatus.ServoPosition}");
                }
                else
                {
                    logger.LogWarning("Could not fetch Fans status");
                }
                

                foreach (var sensor in sensors) {

                    if (types.First(i => i.Id == sensor.TypeId).Source == "Prometheus")
                    {
                        var sensorValue = await prometheusClient.GetSensorValueAsync(config.GetValue<string>("Prometheus:ServerAddress"), sensor.SensorExtID);

                       // Console.WriteLine($"Value: {sensorValue.Value}");

                        if (sensorValue != null)
                        {
                            await db.SensorsHistory.AddAsync(new SensorsHistory()
                            {
                                SensorId = sensor.Id,
                                Value = (double)sensorValue
                            });

                            await db.SaveChangesAsync();
                        }
                    }
                }
            }
        }

        Thread.Sleep(timeout *  1000);

    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
}


















