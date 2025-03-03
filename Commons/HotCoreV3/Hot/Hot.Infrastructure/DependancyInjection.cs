using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.DbContextTables; 
using Hot.Infrastructure.Services;
using Hot.Infrastructure.DbContext;
using Hot.Infrastructure.DbContext.Tables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Infrastructure.RechargeServices.Telecel;
using Hot.Infrastructure.RechargeServices.Econet;
using Hot.Infrastructure.RechargeServices.Telone;
using Hot.Infrastructure.RechargeServices.Nyaradzo;
using Hot.Infrastructure.RechargeServices.Netone;
using Hot.Infrastructure.FactoryServices;
using Hot.Infrastructure.RechargeServices.RechargeAPIServices.Netone;
using Hot.Infrastructure.RechargeServices.ZESA;

namespace Hot.Infrastructure
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddHTTPServices(services);

            AddDbContextServices(services);

            services.AddSingleton<IHotTypeIdentifier, HotTypeIdentifier>();
            RegisterFactories(services);

            AddRechargeServices(services);

            return services;
        }

        private static void RegisterFactories(IServiceCollection services)
        {
            services.AddSingleton<IMessageHandlerFactory, MessageHandlerFactory>();
            services.AddTransient<IRechargeHandlerFactory, RechargeHandlerFactory>();
            services.AddTransient<IDiscountRulesFactory, DiscountRulesFactory>();
            services.AddTransient<IRechargeDataQueryHandlerFactory, RechargeDataQueryHandlerFactory>();
            services.AddTransient<IRechargeUtilityQueryHandlerFactory, RechargeUtilityQueryHandlerFactory>(); 
            services.AddTransient<IRechargeMobileQueryHandlerFactory, RechargeMobileQueryHandlerFactory>();

        }

        private static void AddHTTPServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddTransient<IAPIService, APIService>();
        }

        private static void AddDbContextServices(IServiceCollection services)
        {
            services.AddSingleton<IDbHelper, DbHelper>();
            services.AddSingleton<IDbContext, AppDbContext>();
            services.AddSingleton<IAccesss, Accesss>();
            services.AddSingleton<IAccounts, Accounts>();
            services.AddSingleton<IAddresses, Addresses>();
            services.AddSingleton<IBanks, Banks>();
            services.AddSingleton<IBankTrxs, BankTrxs>();
            services.AddSingleton<IBankTrxBatches, BankTrxBatches>();
            services.AddSingleton<ITemplates, Templates>();
            services.AddSingleton<ISMSs, SMSs>();
            services.AddSingleton<ISMPPs, SMPPs>();
            services.AddSingleton<IAccessWebs, AccessWebs>();
            services.AddSingleton<ITransfers, Transfers>();
            services.AddSingleton<IPayments, Payments>();
            services.AddSingleton<IPaymentTypes, PaymentTypes>();
            services.AddSingleton<IPaymentSources, PaymentSources>();
            services.AddSingleton<IPins, Pins>();
            services.AddSingleton<IPinBatches, PinBatches>();
            services.AddSingleton<IPinBatchTypes, PinBatchTypes>();
            services.AddSingleton<IBankTransactionType, BankTransactionTypes>();
            services.AddSingleton<IBankTransactionStates, BankTransactionStates>();
            services.AddSingleton<IBrands, Brands>();
            services.AddSingleton<IChannels, Channels>();
            services.AddSingleton<IConfigs, Configs>();
            services.AddSingleton<IHotTypes, HotTypes>();
            services.AddSingleton<ILogs, Logs>();
            services.AddSingleton<INetworks, Networks>();
            services.AddSingleton<IRecharges, Recharges>();
            services.AddSingleton<IProfiles, Profiles>();
            services.AddSingleton<IProfileDiscounts, ProfileDiscounts>();
            services.AddSingleton<IRechargePrepaids, RechargePrepaids>();
            services.AddSingleton<ISelfTopUps, SelfTopUps>();
            services.AddSingleton<ISelfTopUpStates, SelfTopUpStates>();
            services.AddSingleton<IStatistics, Statistics>();
            services.AddSingleton<INetworkBalances, NetworkBalances>();
            services.AddSingleton<IBundles, Bundles>();
            services.AddSingleton<IReports, Reports>();
            services.AddSingleton<ILimits, Limits>();
            services.AddSingleton<IWebRequests, WebRequests>();
            services.AddSingleton<ITelecelReconZWL, TelecelReconZWLs>();
            services.AddSingleton<IEconetReconZWL, EconetReconZWLs>();
            services.AddSingleton<IEconetReconUSD, EconetReconUSDs>();
            services.AddSingleton<IProducts, Products>();
            services.AddSingleton<IProductFields, ProductFields>();
            services.AddSingleton<IProductMetaDatas, ProductMetaDatas>();
            services.AddSingleton<IProductMetaDataTypes, ProductMetaDataTypes>();
            services.AddSingleton<IReservation, Reservations>();
            services.AddSingleton<IReservationLog, ReservationLogs>();
        }


        private static void AddRechargeServices(this IServiceCollection services)
        {
            services.AddTransient<IEconetRechargeAPIService, EconetAirtimeRechargeService>();
            services.AddTransient<IEconetRechargeDataAPIService, EconetDataRechargeService>();
            services.AddTransient<INetoneRechargeDataAPIService, NetoneDataRechargeServices>();
            services.AddTransient<IEconetRechargePrepaidAPIService, EconetPrePaidRechargeService>();
            services.AddTransient<INetOneRechargeAPIService, NetoneAirtimeRechargeService>();
            services.AddTransient<ITelecelRechargeAPIService, TelecelAirtimeRechargeService>();
            services.AddTransient<INyaradzoRechargeAPIService, NyaradzoRechargeAPIService>();
            services.AddTransient<ITeloneDataAPIService, TeloneDataRechargeService>();
            services.AddTransient<IZESARechargeAPIService, ZESARechargeAPIService>(); 

        }
    }
}
