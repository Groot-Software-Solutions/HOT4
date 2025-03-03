
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Sage.Application.Common.Interfaces;
using Sage.Application.Common.Models;
using Sage.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Sage.Infrastructure
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            ConfigureHttpClient(services, configuration); 
            ConfigureSageSettings(services, configuration); 
            services.AddTransient<IAPIService, APIService>();
            services.AddTransient<IDbHelper, DbHelper>();
            services.AddTransient<IHotDbContext, HotDbContext>(); 
            services.AddTransient<ISageAPIService, SageAPIService>();

            return services;
        }

        private static void ConfigureSageSettings(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSageServiceOptions(options =>
            {
                options.APICompanyID = configuration.GetValue<string>("SageAPICompanyId");
                options.APIKey = configuration.GetValue<string>("SageAPIKey");
            }); 
            SageSystemOptions.HotAirtimeItemId = configuration.GetValue<int>("HotAirtimeItemId");
            SageSystemOptions.HotZesaItemId = configuration.GetValue<int>("HotZesaItemId"); 
            SageSystemOptions.HotCustomerId = configuration.GetValue<int>("HotCustomerId");
            SageSystemOptions.HotDefaultTaxId = configuration.GetValue<int>("HotDefaultTaxId");
            SageSystemOptions.HotZesaTaxId = configuration.GetValue<int>("HotZesaTaxId");
            SageSystemOptions.HotSalesRepId = configuration.GetValue<int>("HotSalesRepId");
            SageSystemOptions.HotUSDTaxId = configuration.GetValue<int>("HotUSDTaxId");

            SageSystemOptions.HotProfileCategory = new Dictionary<int, int>();
            var dictionary = configuration.GetSection("HotProfileCategory").AsEnumerable();
            dictionary.ToList()
                .ForEach(d =>
                {
                    if (d.Value != null) SageSystemOptions.HotProfileCategory.Add(Convert.ToInt32(d.Key.Replace("HotProfileCategory:", "")), Convert.ToInt32(d.Value));
                });
        }

        private static void ConfigureHttpClient(IServiceCollection services, IConfiguration configuration)
        {
            Func<HttpMessageHandler> configureHandler = () =>
            {
                var bypassCertValidation = configuration.GetValue<bool>("Sage_BypassRemoteCertificateValidation", false);
                var handler = new HttpClientHandler();
                if (bypassCertValidation)
                {
                    handler.ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, x509Certificate2, x509Chain, sslPolicyErrors) => { return true; };
                }
                return handler;
            };

            services.AddHttpClient("Sage", client =>
            {
                client.BaseAddress = new Uri(configuration.GetValue<string>("SageAPIUrl"));
                var APICredentials = Encoding.ASCII.GetBytes(
                    $"{configuration.GetValue<string>("SageAPIUsername")}:{configuration.GetValue<string>("SageAPIPassword")}"
                    );
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(APICredentials));
            })
                .ConfigurePrimaryHttpMessageHandler(configureHandler)
                .AddTransientHttpErrorPolicy(p => p.RetryAsync());
        }

    }
}
