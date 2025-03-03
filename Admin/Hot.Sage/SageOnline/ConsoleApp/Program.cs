using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sage.Application;
using Sage.Infrastructure;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static IConfigurationRoot configuration;

        static void Main()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            serviceProvider.GetService<App>().Run();
        }

        static void ConfigureServices(IServiceCollection serviceCollection)
        {

            // Build configuration
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            serviceCollection.AddLogging(builder =>
            {
                var logger = new LoggerConfiguration()
                         .MinimumLevel.Information()
                         .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                         .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                         .MinimumLevel.Override("System.Net.Http.HttpClient", LogEventLevel.Warning)
                         .WriteTo.Console()
                         .CreateLogger();

                builder.AddSerilog(logger);
            });
            serviceCollection.AddSingleton<IConfiguration>(configuration);
            serviceCollection.AddSingleton(configuration);

            serviceCollection.AddApplication();
            serviceCollection.AddInfrastructure(configuration);

            serviceCollection.AddTransient<App>();
        

        }
    }
}
