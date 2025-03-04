using Hot.Application;
using Hot.Infrastructure;
using Hot.SMPP.Installers;
using Hot.SMPP.Interfaces;
using Hot.SMPP.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.SMPP;
public static class DependancyInjection
{
    public static void AddSMPPService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplication();
        services.AddInfrastructure(configuration);
        services.AddInetLabLicense();
        services.AddSingleton<ISMPPService, SMPPService>();
    }
}
