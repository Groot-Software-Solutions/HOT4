using Hot.Ecocash.Application.Common.Interfaces;
using Hot.Ecocash.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Text;

namespace Hot.Ecocash
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddEcocash(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IEcocashService, EcoCashService>();
            services.AddTransient<IEcocashServiceFactory, EcocashServiceFactory>();
            services.AddHttpClient();
            ConfigureEcocashHttpClients(services, configuration);

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            return services;
        }

        private static void ConfigureEcocashHttpClients(IServiceCollection services, IConfiguration configuration)
        {
            foreach (var account in configuration.GetSection("EcocashAccounts").GetChildren())
            {
                string baseAddress = account.GetSection("EcoCashURL").Value ??"";
                string username = account.GetSection("Username").Value ?? "";
                string password = account.GetSection("Password").Value ?? "";
                string apiName = account.Key;
                services.AddHttpClient(apiName, client =>
                {
                    client.BaseAddress = new Uri(baseAddress);

                    var APICredentials = Encoding.ASCII.GetBytes(
                        $"{username}:{password}"
                        );
                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(APICredentials));

                });

            }
        }
    }
}
