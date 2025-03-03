using Hot.API.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration.Internal;

namespace Hot.BulkRecharge
{
    internal static class Program
    {

        public static IServiceProvider ServiceProvider { get; set; }
       
        static void ConfigureServices()
        {
            var services = new ServiceCollection();
            var Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
            .AddJsonFile("appsettings.json", false)
            .Build();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddHttpClient();
            services.AddHotRechargeAPIClient();
            services.AddTransient<Form1>();
            services.AddTransient<HotRechargeService>();

            ServiceProvider = services.BuildServiceProvider();
        }
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ConfigureServices();
            //ApplicationConfiguration.Initialize();
            //Application.Run(new Form1());
            Application.Run(ServiceProvider.GetService<Form1>());

        }
    }
}