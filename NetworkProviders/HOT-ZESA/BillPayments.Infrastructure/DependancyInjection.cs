
using BillPayments.Application.Common.Interfaces;
using BillPayments.Application.Services;
using BillPayments.Infrastructure.Data;
using BillPayments.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace BillPayments.Infrastructure
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
           
            Func<HttpMessageHandler> configureHandler = () =>
               {
                   var bypassCertValidation = configuration.GetValue<bool>("ZESA_BypassRemoteCertificateValidation",false);
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

            services.AddHttpClient("ZESA", client =>
            {
                client.BaseAddress = new Uri(configuration.GetValue<string>("ZESA_Url"));

                var APICredentials = Encoding.ASCII.GetBytes(
                    $"{configuration.GetValue<string>("ZESA_Username")}:{configuration.GetValue<string>("ZESA_Password")}"
                    );
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(APICredentials));

            }).ConfigurePrimaryHttpMessageHandler(configureHandler);

            services.AddHttpClient("HOT", client =>
            {
                client.BaseAddress = new Uri(configuration.GetValue<string>("HotAPI_Url"));
                client.DefaultRequestHeaders.Add("x-access-code", configuration.GetValue<string>("HotAPI_Username"));
                client.DefaultRequestHeaders.Add("x-access-password", configuration.GetValue<string>("HotAPI_Password"));
                 
            });

            services.AddDbContext<AppDbContext>(options =>
                           options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<IBackgroundTaskService, BackgroundTaskService>();

            services.AddTransient<IBackgroundPaymentService, BackgroundPaymentService>();

            services.AddTransient<IAPIService, APIService>();

            services.AddTransient<ILogger, Logger>();
            return services;
        }


    }
}
