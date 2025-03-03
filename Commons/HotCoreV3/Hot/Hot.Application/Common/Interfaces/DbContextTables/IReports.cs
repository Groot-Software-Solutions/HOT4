namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface IReports: IDbContextTable<Report>
    {
        public Task<OneOf<List<StatementTransaction>, HotDbException>> GetStatementAsync(long AccountID, DateTime StartDate, DateTime EndDate);
        public OneOf<List<StatementTransaction>, HotDbException> GetStatement(long AccountID, DateTime StartDate, DateTime EndDate);
        public Task<OneOf<decimal, HotDbException>> GetStartingBalanceAsync(long AccountID, DateTime StartDate);
        public OneOf<decimal, HotDbException> GetStartingBalance(long AccountID, DateTime StartDate);


        public Task<OneOf<List<PaymentResult>, HotDbException>> GetPaymentsAsync( DateTime StartDate, DateTime EndDate, int ReportTypeId, long? AccountId, int? PaymentTypeId, int? BankId );
        public OneOf<List<PaymentResult>, HotDbException> GetPayments(DateTime StartDate, DateTime EndDate, int ReportTypeId, long? AccountId, int? PaymentTypeId, int? BankId);

        public Task<OneOf<List<StatementTransaction>, HotDbException>> GetTransactionsAsync(DateTime StartDate, DateTime EndDate, int ReportTypeId, long? AccountId, int? WalletTypeId, int? BankId );
        public OneOf<List<StatementTransaction>, HotDbException> GetTransactions(DateTime StartDate, DateTime EndDate, int ReportTypeId, long? AccountId, int? WalletTypeId, int? BankId);

        public Task<OneOf<List<PeriodicStatsResult>, HotDbException>> GetStatsAsync(DateTime StartDate, DateTime EndDate, int ReportTypeId, long? NetworkID, int? AccountId);
        public OneOf<List<PeriodicStatsResult>, HotDbException> GetStats(DateTime StartDate, DateTime EndDate, int ReportTypeId, long? NetworkID, int? AccountId);

        public Task<OneOf<List<EconetStatsResult>, HotDbException>> GetEconetStatsAsync(DateTime StartDate, DateTime EndDate);
        public OneOf<List<EconetStatsResult>, HotDbException> GetEconetStats(DateTime StartDate, DateTime EndDate);


        public Task<OneOf<List<ProfileDiscountResult>, HotDbException>> GetProfileDiscountsAsync(int ReportId, int? WalletId);
        public OneOf<List<ProfileDiscountResult>, HotDbException> GetProfileDiscounts(int ReportId, int? WalletId);
    }
}
