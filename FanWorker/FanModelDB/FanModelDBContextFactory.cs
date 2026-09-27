using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace FanModelDB
{
    public class FanModelDBContextFactory  : IDesignTimeDbContextFactory<FanModelDBContext>
    {
        public FanModelDBContext CreateDbContext(string[] args)
        {
            string c = Directory.GetCurrentDirectory();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);

            IConfiguration config = builder.Build();



            string connectionStringIs = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<FanModelDBContext>();

            optionsBuilder.UseNpgsql(connectionStringIs);

            optionsBuilder.ConfigureWarnings(warnings =>
            {
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning);
            });


            return new FanModelDBContext(optionsBuilder.Options);
        }
    }
}
