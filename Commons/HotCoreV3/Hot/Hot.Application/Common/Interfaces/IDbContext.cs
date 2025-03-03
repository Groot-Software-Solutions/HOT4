using Hot.Application.Common.Interfaces.DbContextTables;
using System.Data;

namespace Hot.Application.Common.Interfaces
{
    public interface IDbContext
    {
        public IAccesss Accesss { get; set; }
        public IAccounts Accounts { get; set; }
        public IAddresses Addresses { get; set; }
        public ITemplates Templates { get; set; }
        public ISMSs SMSs { get; set; }
        public IBanks Banks { get; set; }
        public IBankTrxs BankTrxs { get; set; }
        public IBankTrxBatches BankTrxBatches { get; set; }
        public ISMPPs SMPPs { get; set; }
        public IAccessWebs AccessWebs { get; set; }
        public IPayments Payments { get; set; }
        public ITransfers Transfers { get; set; }
        public IPaymentSources PaymentSources { get; set; }
        public IPaymentTypes PaymentTypes { get; set; }
        public IPins Pins { get; set; }
        public IPinBatches PinBatches { get; set; }
        public IPinBatchTypes PinBatchTypes { get; set; }
        public IBankTransactionType BankTransactionTypes { get; set; }
        public IBankTransactionStates BankTransactionStates { get; set; }
        public IBrands Brands { get; set; }
        public IChannels Channels { get; set; }
        public IConfigs Configs { get; set; }
        public IHotTypes HotTypes { get; set; }
        public ILogs Logs { get; set; }
        public INetworks Networks { get; set; }
        public IProfiles Profiles { get; set; }
        public IProfileDiscounts ProfileDiscounts { get; set; }
        public IRecharges Recharges { get; set; }
        public IRechargePrepaids RechargePrepaids { get; set; }
        public ISelfTopUps SelfTopUps { get; set; }
        public IStatistics Statistics { get; set; }
        public INetworkBalances NetworkBalance { get; set; }
        public IBundles Bundles { get; set; }
        public IReports Report { get; set; }
        public ILimits Limits { get; set; }
        public IWebRequests WebRequests { get; set; }

        public ITelecelReconZWL TelecelReconZWLs { get; set; }
        public IEconetReconZWL EconetReconZWLs { get; set; }
        public IEconetReconUSD EconetReconUSDs { get; set; }
        public IProducts Products { get; set; }
        public IProductFields ProductFields { get; set; }
        public IProductMetaDatas ProductMetaDatas { get; set; }
        public IProductMetaDataTypes ProductMetaDataTypes { get; set; }

        public IReservation Reservations { get; set; }
        public IReservationLog ReservationLogs { get; set; }



        public Task<Tuple<IDbConnection, IDbTransaction>?> BeginTransactionAsync();
        public bool CompleteTransaction(IDbTransaction dbtransaction);
        public bool RollbackTransaction(IDbTransaction dbtransaction);
    }
}