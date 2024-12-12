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
            services.AddTransient<IAnswerRepository, AnswerRepository>();
            services.AddTransient<IBankRepository, BankRepository>();
            services.AddTransient<IChannelRepository, ChannelRepository>();
            services.AddTransient<IConfigRepository, ConfigRepository>();
            services.AddTransient<IHotTypeRepository, HotTypeRepository>();
            services.AddTransient<ILimitRepository, LimitRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddTransient<IPaymentSourceRepository, PaymentSourceRepository>();
            services.AddTransient<IPaymentTypeRepository, PaymentTypeRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IProfileDiscountRepository, ProfileDiscountRepository>();
            services.AddTransient<IProfileRepository, ProfileRepository>();
            services.AddTransient<IRechargePrepaidRepository, RechargePrepaidRepository>();
            services.AddTransient<IRechargeRepository, RechargeRepository>();
            services.AddTransient<IRegistrationRepository, RegistrationRepository>();
            services.AddTransient<ISMPPRepository, SMPPRepository>();
            services.AddTransient<ISMSRepository, SMSRepository>();
            services.AddTransient<IStateRepository, StateRepository>();
            services.AddTransient<ISubscriberRepository, SubscriberRepository>();
            services.AddTransient<ITemplateRepository, TemplateRepository>();

            return services;
        }
    }
}
