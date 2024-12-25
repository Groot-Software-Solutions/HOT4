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
            services.AddTransient<IBankTrxRepository, BankTrxRepository>();
            services.AddTransient<IBankTrxBatchRepository, BankTrxBatchRepository>();
            services.AddTransient<IBankTrxStateRepository, BankTrxStateRepository>();
            services.AddTransient<IBankTrxTypeRepository, BankTrxTypeRepository>();

            services.AddTransient<IBrandRepository, BrandRepository>();
            services.AddTransient<IBundleRepository, BundleRepository>();
            services.AddTransient<IChannelRepository, ChannelRepository>();

            services.AddTransient<ICommonRepository, CommonRepository>();
            services.AddTransient<IConfigRepository, ConfigRepository>();
            services.AddTransient<IHotTypeRepository, HotTypeRepository>();
            services.AddTransient<ILimitRepository, LimitRepository>();

            services.AddTransient<ILogRepository, LogRepository>();
            services.AddTransient<INetworkBalanceRepository, NetworkBalanceRepository>();
            services.AddTransient<INetworkRepository, NetworkRepository>();

            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddTransient<IPaymentSourceRepository, PaymentSourceRepository>();
            services.AddTransient<IPaymentTypeRepository, PaymentTypeRepository>();

            services.AddTransient<IPinBatchRepository, PinBatchRepository>();
            services.AddTransient<IPinBatchTypeRepository, PinBatchTypeRepository>();
            services.AddTransient<IPinRepository, PinRepository>();

            services.AddTransient<IProductFieldRepository, ProductFieldRepository>();
            services.AddTransient<IProductMetaDataRepository, ProductMetaDataRepository>();
            services.AddTransient<IProductMetaDataTypeRepository, ProductMetaDataTypeRepository>();
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
