using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Infrastructure.Services;
using Infrastructure.Handlers;
using Infrastructure.Extensions;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(  this IServiceCollection services,  IConfiguration configuration)
        {
            services.AddHttpClient();
            services.AddHttpClient("WhatsApp",
                c => {
                    c.BaseAddress = new Uri(configuration.GetValue<string>("WhatsAppWebURL"));
                    var headers = new HeaderDictionary();
                    c.DefaultRequestHeaders.Add("x-maytapi-key", configuration.GetValue<string>("WhatsAppWebAPIKey"));
                });
            
            services.AddSingleton<ConfigHelper>();
            services.AddSingleton<IDbHelper, DbHelper>();
            services.AddSingleton<IDbContext, DbContext>();
            services.AddTransient<IAPIHelper, APIHelper>();
            services.AddSingleton<IMessageHandler, MessageHandler>();
            services.AddSingleton<IRechargeHelper, RechargeService>();

            services.AddSingleton<IDealerService, DealerService>();
            services.AddSingleton<IWhatsAppService, TwilioService>();
            services.AddSingleton<ITemplateHelper, TemplateHelper>();

            services.AddSingleton<IConfigHelper, ConfigHelper>();
            services.AddSingleton<IMessageProccessor, MessageProccessor>();
            services.AddSingleton<ILogger, Logger>();
            services.AddTransient<TwilioService>();
            services.AddTransient<MaytApiService>();

            //services.AddRechargeService(options =>
            //{
            //    options.BaseUrl = configuration.GetValue<string>("HotAPIUrl");
            //    options.AccessCode = configuration.GetValue<string>("HotAccessCode");
            //    options.AccessPassword = configuration.GetValue<string>("HotAccessPassword"); 
            //});
            services.AddHttpClient("HOT", client =>
            {
                client.BaseAddress = new Uri(configuration.GetValue<string>("HotAPIUrl"));
                client.DefaultRequestHeaders.Add("x-access-code", configuration.GetValue<string>("HotAccessCode"));
                client.DefaultRequestHeaders.Add("x-access-password", configuration.GetValue<string>("HotAccessPassword"));

            });

            return services;
        }
    }
}
