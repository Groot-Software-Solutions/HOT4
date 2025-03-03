using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Hot.Infrastructure.Services
{
    public class AppDbContext : IDbContext
    {
        private readonly IDbHelper dbHelper;

        public AppDbContext(IDbHelper dbHelper, IAccesss accesss, IAccessWebs accessWebs, IAccounts accounts, IAddresses addresses, ITemplates templates, ISMSs sMSs, IBanks banks, IBankTrxs bankTrxs, IBankTrxBatches bankTrxBatches, ISMPPs sMPPs, IPayments payments, ITransfers transfers, IPaymentSources paymentSources, IPaymentTypes paymentTypes, IPins pins, IPinBatches pinBatches, IPinBatchTypes pinBatchTypes, IBankTransactionType bankTransactionTypes, IBankTransactionStates bankTransactionStates, IBrands brands, IChannels channels, IConfigs configs, IHotTypes hotTypes, ILogs logs, INetworks networks, IProfiles profiles, IProfileDiscounts profileDiscounts, IRecharges recharges, IRechargePrepaids rechargePrepaids, ISelfTopUps selfTopUps, ISelfTopUpStates selfTopUpStates, IStatistics statistics, INetworkBalances networkBalance, IBundles bundles, IReports report, ILimits limits, IWebRequests webRequests, ITelecelReconZWL telecelReconZWLs, IEconetReconZWL econetReconZWLs, IEconetReconUSD econetReconUSDs, IProducts products, IProductFields productFields, IProductMetaDatas productMetaDatas, IProductMetaDataTypes productMetaDataTypes, IReservation reservations, IReservationLog reservationLogs)
        {
            this.dbHelper = dbHelper;
            Accesss = accesss;
            AccessWebs = accessWebs;
            Accounts = accounts;
            Addresses = addresses;
            Templates = templates;
            SMSs = sMSs;
            Banks = banks;
            BankTrxs = bankTrxs;
            BankTrxBatches = bankTrxBatches;
            SMPPs = sMPPs;
            Payments = payments;
            Transfers = transfers;
            PaymentSources = paymentSources;
            PaymentTypes = paymentTypes;
            Pins = pins;
            PinBatches = pinBatches;
            PinBatchTypes = pinBatchTypes;
            BankTransactionTypes = bankTransactionTypes;
            BankTransactionStates = bankTransactionStates;
            Brands = brands;
            Channels = channels;
            Configs = configs;
            HotTypes = hotTypes;
            Logs = logs;
            Networks = networks;
            Profiles = profiles;
            ProfileDiscounts = profileDiscounts;
            Recharges = recharges;
            RechargePrepaids = rechargePrepaids;
            SelfTopUps = selfTopUps;
            SelfTopUpStates = selfTopUpStates;
            Statistics = statistics;
            NetworkBalance = networkBalance;
            Bundles = bundles;
            Report = report;
            Limits = limits;
            WebRequests = webRequests;
            TelecelReconZWLs = telecelReconZWLs;
            EconetReconZWLs = econetReconZWLs;
            EconetReconUSDs = econetReconUSDs;
            Products = products;
            ProductFields = productFields;
            ProductMetaDatas = productMetaDatas;
            ProductMetaDataTypes = productMetaDataTypes;
            Reservations = reservations;
            ReservationLogs = reservationLogs;
        }

        public IAccesss Accesss { get; set; }
        public IAccessWebs AccessWebs { get; set; }
        public IAccounts Accounts { get; set; }
        public IAddresses Addresses { get; set; }
        public ITemplates Templates { get; set; }
        public ISMSs SMSs { get; set; }
        public IBanks Banks { get; set; }
        public IBankTrxs BankTrxs { get; set; }
        public IBankTrxBatches BankTrxBatches { get; set; }
        public ISMPPs SMPPs { get; set; }
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
        public ISelfTopUpStates SelfTopUpStates { get; set; }
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

        public bool CompleteTransaction(IDbTransaction dbtransaction)
        {
            var result = dbHelper.CommitTransaction(dbtransaction);
            return result.IsT0 && result.AsT0;
        }

        public async Task<Tuple<IDbConnection, IDbTransaction>?> BeginTransactionAsync()
        {
            var result = await dbHelper.BeginTransaction();
            if (result.IsT1) return null;
            return result.AsT0;
        }

        public bool RollbackTransaction(IDbTransaction dbtransaction)
        {
            var result = dbHelper.RollBackTransaction(dbtransaction);
            return result.IsT0 && result.AsT0;
        }

    }
}
