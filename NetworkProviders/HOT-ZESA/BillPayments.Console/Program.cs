using BillPayments.Application;
using BillPayments.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace BillPayments.Console
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


            serviceCollection.AddSingleton<IConfiguration>(configuration);
            serviceCollection.AddSingleton<IConfigurationRoot>(configuration);

            serviceCollection.AddApplication();
            serviceCollection.AddInfrastructure(configuration);

            serviceCollection.AddTransient<App>();


        }

    }
}
