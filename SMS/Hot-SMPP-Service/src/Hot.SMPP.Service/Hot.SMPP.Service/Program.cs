using Hot.Domain.Entities;
using Hot.SMPP.Service;
using Hot.SMPP;

var config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", optional: true)
                        .AddUserSecrets<Program>()
                        .Build();
var Version = "4.6.0";
IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(o =>
    {
        o.ServiceName = $"Hot.SMPP.Service {Version}";
    })
    .ConfigureServices(services =>
    {
        services.AddSMPPService(config);
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
