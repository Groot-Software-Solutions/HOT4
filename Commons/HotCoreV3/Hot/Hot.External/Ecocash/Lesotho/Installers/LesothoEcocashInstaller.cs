using Hot.Ecocash.Application.Common.Interfaces;
using Hot.Ecocash.Infrastructure.Services;
using Hot.Ecocash.Lesotho.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Ecocash.Lesotho.Installers;
public static class LesothoEcocashInstaller
{
    public static IServiceCollection AddEcocashLesotho(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IEcocashService, EcoCashLesothoService>();
        services.AddTransient<IEcocashServiceFactory, EcocashLesothoServiceFactory>();

        foreach (var account in configuration.GetSection("EcocashAccounts").GetChildren())
        {
            var baseAddress = account.GetSection("EcoCashURL").Value ?? "";
            services.AddRefitClient<IEcocashLesothoAPI>()
                    .ConfigureHttpClient(c =>
                    {
                        c.BaseAddress = new Uri(baseAddress);
                    });
            services.AddRefitClient<IEcocashLesothoIdentityAPI>()
                    .ConfigureHttpClient(c =>
                    {
                        c.BaseAddress = new Uri(baseAddress);
                    });
        }

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        return services;
    }
}

