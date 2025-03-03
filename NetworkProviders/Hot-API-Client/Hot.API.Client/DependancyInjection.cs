using Hot.API.Client.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Hot.API.Client
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddHotRechargeAPIClient(this IServiceCollection services)
        {
            
            services.AddTransient<HotAPIClient>();
            services.AddSingleton<IAPIService, APIHelper>();
            return services;
        }
    }
}
