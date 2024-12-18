using Hot4.Repository.Abstract;
using Hot4.Repository.Concrete;
using Microsoft.Extensions.DependencyInjection;

namespace Hot4.Repository
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddItemServices(this IServiceCollection services)
        {

            services.AddTransient<IAccessRepository, AccessRepository>();
            services.AddTransient<IAccessWebRepository, AccessWebRepository>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IAddressRepository, AddressRepository>();
            return services;
        }
    }
}
