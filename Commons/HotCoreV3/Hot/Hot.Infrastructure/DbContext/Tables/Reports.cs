using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Application.Common.Models;
using Hot.Domain.Entities;
using OneOf;

namespace Hot.Infrastructure.DbContext.Tables
{
    public class Reports : Table<Reports>, IReports
    {
        public Reports(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x"; 
        }

        public OneOf<decimal, HotDbException> GetStartingBalance(long AccountID, DateTime StartDate)
        {
            return GetStartingBalanceAsync(AccountID, StartDate).Result;
        }
        public async Task<OneOf<decimal, HotDbException>> GetStartingBalanceAsync(long AccountID, DateTime StartDate)
        {
            return await dbHelper.QuerySingle<decimal>($"{GetSPPrefix()}_GetStartingBalance @AccountID,@StartDate", new { AccountID, StartDate });
        }

        public OneOf<List<StatementTransaction>, HotDbException> GetStatement(long AccountID, DateTime StartDate, DateTime EndDate)
        {
            return GetStatementAsync(AccountID, StartDate, EndDate).Result;
        }

        public async Task<OneOf<List<StatementTransaction>, HotDbException>> GetStatementAsync(long AccountID, DateTime StartDate, DateTime EndDate)
        {
            return await dbHelper.Query<StatementTransaction>($"{GetSPPrefix()}_GetStatement @AccountID, @StartDate,@EndDate", new {AccountID, StartDate, EndDate });
        }
 
        public async Task<OneOf<List<ProfileDiscountResult>, HotDbException>> GetProfileDiscountsAsync(int ReportId, int? WalletId)
        {
            return await dbHelper.Query<ProfileDiscountResult>($"{GetSPPrefix()}_GetProfileDiscountsByWalletId @ReportId, @WalletId", new { ReportId, WalletId});
        }
        public OneOf<List<ProfileDiscountResult>, HotDbException> GetProfileDiscounts(int ReportId, int? WalletId)
        {
            return GetProfileDiscountsAsync(ReportId, WalletId).Result;
        }

        public async Task<OneOf<List<PaymentResult>, HotDbException>> GetPaymentsAsync(DateTime StartDate, DateTime EndDate, int ReportTypeId, long? AccountId, int? PaymentTypeId, int? BankId)
        {
            return await dbHelper.Query<PaymentResult>($"{GetSPPrefix()}_GetPayments @StartDate,@EndDate, @ReportTypeId, @AccountId, @PaymentTypeId, @BankId",
               new { StartDate, EndDate, ReportTypeId,  AccountId,  PaymentTypeId,BankId});
        }

        public OneOf<List<PaymentResult>, HotDbException> GetPayments(DateTime StartDate, DateTime EndDate, int ReportTypeId, long? AccountId, int? PaymentTypeId, int? BankId)
        {
            return GetPaymentsAsync(StartDate, EndDate, ReportTypeId, AccountId, PaymentTypeId, BankId).Result;
        }

        public async Task<OneOf<List<StatementTransaction>, HotDbException>> GetTransactionsAsync(DateTime StartDate, DateTime EndDate, int ReportTypeId, long? AccountId, int? WalletTypeId, int? BankId)
        {
            return await dbHelper.Query<StatementTransaction>($"{GetSPPrefix()}_GetTransactions @StartDate,@EndDate, @ReportTypeId, @AccountId, @WalletTypeId, @BankId",
               new { StartDate, EndDate, ReportTypeId, AccountId, WalletTypeId, BankId });
        }

        public OneOf<List<StatementTransaction>, HotDbException> GetTransactions(DateTime StartDate, DateTime EndDate, int ReportTypeId, long? AccountId, int? WalletTypeId, int? BankId)
        {
            return GetTransactionsAsync(StartDate, EndDate, ReportTypeId, AccountId, WalletTypeId, BankId).Result;
        }

        public async Task<OneOf<List<PeriodicStatsResult>, HotDbException>> GetStatsAsync(DateTime StartDate, DateTime EndDate, int ReportTypeId, long? NetworkID, int? AccountId)
        {
            return await dbHelper.Query<PeriodicStatsResult>($"{GetSPPrefix()}_GetStats @StartDate,@EndDate, @ReportTypeId, @NetworkID, @AccountId",
                new { StartDate, EndDate, ReportTypeId, NetworkID, AccountId });
        }

        public OneOf<List<PeriodicStatsResult>, HotDbException> GetStats(DateTime StartDate, DateTime EndDate, int ReportTypeId, long? NetworkID, int? AccountId)
        {
            return GetStatsAsync(StartDate, EndDate, ReportTypeId, NetworkID, AccountId).Result;
        }

        public async Task<OneOf<List<EconetStatsResult>, HotDbException>> GetEconetStatsAsync(DateTime StartDate, DateTime EndDate)
        {
            return await dbHelper.Query<EconetStatsResult>($"{GetSPPrefix()}_GetEconetStats @StartDate,@EndDate",
               new { StartDate, EndDate });
        }

        public OneOf<List<EconetStatsResult>, HotDbException> GetEconetStats(DateTime StartDate, DateTime EndDate)
        {
            return GetEconetStatsAsync(StartDate, EndDate).Result;
        }
    }
}
