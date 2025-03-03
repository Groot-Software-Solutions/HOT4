using TelOne.Application.Common.Interfaces;
using TelOne.Infrastructure.Data;
using TelOne.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http; 
using Polly;

namespace TelOne.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            ConfigureHttpServices(services, configuration);

            services.AddDbContext<AppDbContext>(options =>
                           options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<IAPIService, APIService>(); 
            services.AddTransient<ILogger, Logger>();
            services.AddTransient<ITelOneService, TelOneService>();

            return services;
        }

        private static void ConfigureHttpServices(IServiceCollection services, IConfiguration configuration)
        {
            Func<HttpMessageHandler> configureHandler = () =>
            {
                var bypassCertValidation = configuration.GetValue<bool>("TelOne_BypassRemoteCertificateValidation", false);
                var handler = new HttpClientHandler();
                if (bypassCertValidation)
                {
                    handler.ServerCertificateCustomValidationCallback = (httpRequestMessage, x509Certificate2, x509Chain, sslPolicyErrors) =>
                    {
                        return true;
                    };
                }
                return handler;
            };

            services.AddHttpClient("TelOne", client =>
            {
                client.BaseAddress = new Uri(configuration.GetValue<string>("TelOne_Url"));

                //var APICredentials = Encoding.ASCII.GetBytes(
                //    $"{configuration.GetValue<string>("TelOne_Username")}:{configuration.GetValue<string>("TelOne_Password")}"
                //    );
                //client.DefaultRequestHeaders.Authorization =
                //    new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(APICredentials));

            })
                .ConfigurePrimaryHttpMessageHandler(configureHandler)
                .AddTransientHttpErrorPolicy(p => p.RetryAsync());

            services.AddHttpClient("HOT", client =>
            {
                client.BaseAddress = new Uri(configuration.GetValue<string>("HotAPI_Url"));
                client.DefaultRequestHeaders.Add("x-access-code", configuration.GetValue<string>("HotAPI_Username"));
                client.DefaultRequestHeaders.Add("x-access-password", configuration.GetValue<string>("HotAPI_Password"));
            });


        }


    }
}
