using FanModelDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Start...");

var db_factory = new FanModelDBContextFactory();

var db =  db_factory.CreateDbContext(null);

await db.Database.MigrateAsync();

Console.WriteLine("Get Config...");














