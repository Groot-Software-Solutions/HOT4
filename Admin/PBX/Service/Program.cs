using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Application;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using System.IO;
using Serilog;


namespace Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File($"{Directory.GetParent(AppContext.BaseDirectory).FullName}\\log.txt")
                .WriteTo.Console()
                .CreateLogger();
            try
            {
                Log.Information("Starting up the servive");
                CreateHostBuilder(args).Build().Run();
                return;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to start service");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            } 
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
               .AddJsonFile("appsettings.json", false)
               .Build();

            return Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IConfiguration>(configuration);
                    services.AddSingleton(configuration);
                    services.AddApplication();
                    services.AddInfrastructure(configuration);
                    services.AddHostedService<Worker>();

                })
                .UseSerilog();
        }
    }
}
