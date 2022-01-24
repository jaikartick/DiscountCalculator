using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscountCalculator.DomainModel.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscountController
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetService<AppDbContext>() as DbContext;
                    context.Database.EnsureCreatedAsync();
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
                    logger.LogError(ex, $"Error occured while migrating the database");
                }
                host.Run();
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            try
            {

                var hostBuilder = Host.CreateDefaultBuilder(args)
                    .ConfigureLogging(logging => logging.AddConsole())
                    .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
                return hostBuilder;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
