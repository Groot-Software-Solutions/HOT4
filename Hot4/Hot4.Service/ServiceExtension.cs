using Hot4.Repository.Abstract;
using Hot4.Repository.Concrete;
using Hot4.Service.Abstract;
using Hot4.Service.Concrete;
using Microsoft.Extensions.DependencyInjection;

namespace Hot4.Service
{
    public static class ServiceExtension
    {

        public static IServiceCollection AddItemServices(this IServiceCollection services)
        {

            services.AddTransient<IAccessRepository, AccessRepository>();
            services.AddTransient<IAccessWebRepository, AccessWebRepository>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IAddressRepository, AddressRepository>();
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
            services.AddTransient<IReservationLogRepository, ReservationLogRepository>();
            services.AddTransient<IReservationRepository, ReservationRepository>();
            services.AddTransient<IReservationStateRepository, ReservationStateRepository>();
            services.AddTransient<ISelfTopUpRepository, SelfTopUpRepository>();
            services.AddTransient<ISelfTopUpStateRepository, SelfTopUpStateRepository>();
            services.AddTransient<ISMPPRepository, SMPPRepository>();
            services.AddTransient<ISMSRepository, SMSRepository>();
            services.AddTransient<IStateRepository, StateRepository>();
            services.AddTransient<IStatisticsRepository, StatisticsRepository>();
            services.AddTransient<ITemplateRepository, TemplateRepository>();
            services.AddTransient<ITransferRepository, TransferRepository>();
            services.AddTransient<IWebRequestRepository, WebRequestRepository>();
            services.AddTransient<IBankvPaymentRepository, BankvPaymentRepository>();
            services.AddTransient<ISubscriberRepository, SubscriberRepository>();
            services.AddTransient<IWalletTypeRepository, WalletTypeRepository>();
            services.AddTransient<IStockDataRepository, StockDataRepository>();

            services.AddTransient<IBankTrxBatchService, BankTrxBatchService>();
            services.AddTransient<IBankTrxService, BankTrxService>();
            services.AddTransient<IBankTrxStateService, BankTrxStateService>();
            services.AddTransient<IBankTrxTypeService, BankTrxTypeService>();
            services.AddTransient<IBankvPaymentService, BankvPaymentService>();
            services.AddTransient<IBrandService, BrandService>();
            services.AddTransient<ISMPPService, SMPPService>();
            services.AddTransient<ISMSService, SMSService>();
            services.AddTransient<IStateService, StateService>();
            services.AddTransient<IStatisticsService, StatisticsService>();
            services.AddTransient<ISubscriberService, SubscriberService>();
            services.AddTransient<ITemplateService, TemplateService>();
            services.AddTransient<ITransferService, TransferService>();
            services.AddTransient<IWalletTypeService, WalletTypeService>();
            services.AddTransient<IWebRequestService, WebRequestService>();
            services.AddTransient<IPinBatchTypeService, PinBatchTypeService>();
            services.AddTransient<IPinBatchService, PinBatchService>();
            services.AddTransient<IPinService, PinService>();


            return services;
        }
    }
}
