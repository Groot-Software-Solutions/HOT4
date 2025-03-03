//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace App
//{
//    static class Program
//    {
//        /// <summary>
//        ///  The main entry point for the application.
//        /// </summary>
//        [STAThread]
//        static void Main()
//        {
//            Application.SetHighDpiMode(HighDpiMode.SystemAware);
//            Application.EnableVisualStyles();
//            Application.SetCompatibleTextRenderingDefault(false);
//            Application.Run(new Form1());
//        }
//    }
//}
using App;
using Hot.API.Client;
using Microsoft.Extensions.DependencyInjection;  // provides DI 
using System;
using System.Windows.Forms;

public static class Program
{ 
    public static IServiceProvider ServiceProvider { get; set; }

    static void ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddHttpClient();
        services.AddHotRechargeAPIClient();
        services.AddTransient<Form1>();
        services.AddTransient<HotRechargeService>();
        
        ServiceProvider = services.BuildServiceProvider();
    }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        ConfigureServices(); 
        Application.Run(ServiceProvider.GetService<Form1>());
    }
}
