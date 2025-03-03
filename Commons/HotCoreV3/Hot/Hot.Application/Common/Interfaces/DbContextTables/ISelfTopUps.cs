namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface ISelfTopUps : IDbContextTable<SelfTopUp>
        ,IDbCanAdd<SelfTopUp>
        ,IDbCanUpdate<SelfTopUp> 
    {
        public Task<OneOf<List<SelfTopUp>, HotDbException>> ListPendingRechargeAsync();
        public OneOf<List<SelfTopUp>, HotDbException> ListPendingRecharge();
        public Task<OneOf<SelfTopUp, HotDbException>> GetByBankTrxIdAsync(long BankTrxId);
        public OneOf<SelfTopUp, HotDbException> GetByBankTrxId(long BankTrxId);
    }
}
