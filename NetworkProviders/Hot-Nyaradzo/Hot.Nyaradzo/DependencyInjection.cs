using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Nyaradzo.Application.Common.Interfaces;
using Hot.Nyaradzo.Application.Common.Models;
using Hot.Nyaradzo.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Text;


namespace Hot.Nyaradzo
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddNyaradzo(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<INyaradzoService, NyaradzoService>();
            services.AddTransient<INyaradzoServiceFactory, NyaradzoServiceFactory>();
            services.AddTransient<INyaradzoRechargeAPIService, NyaradzoAPIService>();

            services.AddHttpClient("Nyaradzo", client =>
            {
                var config = configuration.GetSection("Nyaradzo").Get<ServiceOptions>();
                client.BaseAddress = new Uri(config.BaseUrl);
                client.DefaultRequestHeaders.Add("Accept","application/json");
                client.DefaultRequestHeaders.Add("authKey", config.AuthKey);
                client.DefaultRequestHeaders.Add("bankCode", config.BankCode); 
            });
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            return services;
        }

    }
}
