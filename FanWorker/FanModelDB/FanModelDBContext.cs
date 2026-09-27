using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;

namespace FanModelDB
{
    public class FanModelDBContext : DbContext
    {

        public FanModelDBContext(DbContextOptions<FanModelDBContext> options) :
            base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql();



        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Period> Periods { get; set; }

        public DbSet<SensorType> SensorTypes { get; set; }

        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<SensorsHistory> SensorsHistory { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Profile>().HasData(
               new Profile { 
                               Id=1, Name="Обычный", 
                               IsActive=true, Order=1, 
                               createAt = new DateTime(), 
                               updateAt = new DateTime() 
                           }
            );

            modelBuilder.Entity<SensorType>().HasData(
               new SensorType { 
                   Id = 1, 
                   Name = "Внешний",  
                   createAt = new DateTime(), 
                   updateAt = new DateTime() 
               }
            );

            modelBuilder.Entity<Period>().HasData(
                new Period { Id = 1, ProfileId = 1, Name = "Вечер", MaxSpeed = 8, Start = new DateTime(2000, 1, 1, 18, 00, 00), End = new DateTime(2000, 1, 1, 23, 59, 59), createAt = new DateTime(), updateAt = new DateTime() },
                new Period { Id = 2, ProfileId = 1, Name = "Ночь", MaxSpeed = 4, Start = new DateTime(2000, 1, 1, 0, 00, 00), End = new DateTime(2000, 1, 1, 8, 0, 0), createAt = new DateTime(), updateAt = new DateTime() },
                new Period { Id = 3, ProfileId = 1, Name = "День", MaxSpeed = 8, Start = new DateTime(2000, 1, 1, 8, 00, 00), End = new DateTime(2000, 1, 1, 18, 0, 0), createAt = new DateTime(), updateAt = new DateTime() }                     
            );

            modelBuilder.Entity<Settings>().HasData(
                new Settings
                {
                    Id = 1,
                    createAt = new DateTime(),
                    updateAt = new DateTime(),
                    GivenTemperature = 25.5,
                    MaxSpeed = 14,
                    PeriodUpdete = 60,
                    ProfileId = 1,
                    Name = "Стандартные",
                    isAuto = false
                }
            );
        }
    }
}
