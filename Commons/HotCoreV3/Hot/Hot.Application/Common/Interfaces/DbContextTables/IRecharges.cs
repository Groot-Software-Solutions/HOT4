namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface IRecharges : IDbContextTable<Recharge>
       , IDbCanAdd<Recharge>
       , IDbCanUpdate<Recharge>
       , IDbCanSearch<Recharge>
       , IDbCanGetById<Recharge> 
    {
        public Task<OneOf<List<Recharge>, HotDbException>> SelectAggregatorAsync(long AccountId, DateTime StartDate, DateTime EndDate);
        public Task<OneOf<List<Recharge>, HotDbException>> PendingAsync(List<int> BrandIds);
        [Obsolete("Please use PendingAsync(List<int>) instead as this function may not return all the transaction for that Network.")]
        public Task<OneOf<List<Recharge>, HotDbException>> PendingAfricomAsync();
        [Obsolete("Please use PendingAsync(List<int>) instead as this function may not return all the transaction for that Network.")]
        public Task<OneOf<List<Recharge>, HotDbException>> PendingEconetAsync();
        [Obsolete("Please use PendingAsync(List<int>) instead as this function may not return all the transaction for that Network.")]
        public Task<OneOf<List<Recharge>, HotDbException>> PendingNetOneAsync();
        [Obsolete("Please use PendingAsync(List<int>) instead as this function may not return all the transaction for that Network.")]
        public Task<OneOf<List<Recharge>, HotDbException>> PendingOtherAsync();
        public Task<OneOf<Recharge, HotDbException>> WebDuplicateAsync(long AccessId, string Mobile, decimal Amount);
        public Task<OneOf<List<Recharge>, HotDbException>> FindByAccountAsync(long AccountId, string? Mobile);
        public Task<OneOf<List<RechargeResultModel>, HotDbException>> FindByMobileAsync(string? Mobile);

        public Task<OneOf<List<RechargeResultModel>, HotDbException>> FindOldAsync(string? Filter);

        public OneOf<List<Recharge>, HotDbException> SelectAggregator(long AccountId, DateTime StartDate, DateTime EndDate);
        public OneOf<List<Recharge>, HotDbException> Pending(List<int> BrandIds);
        [Obsolete("Please use Pending(List<int>) instead as this function may not return all the transaction for that Network.")]
        public OneOf<List<Recharge>, HotDbException> PendingAfricom();
        [Obsolete("Please use Pending(List<int>) instead as this function may not return all the transaction for that Network.")]
        public OneOf<List<Recharge>, HotDbException> PendingEconet();
        [Obsolete("Please use Pending(List<int>) instead as this function may not return all the transaction for that Network.")]
        public OneOf<List<Recharge>, HotDbException> PendingNetOne();
        [Obsolete("Please use Pending(List<int>) instead as this function may not return all the transaction for that Network.")]
        public OneOf<List<Recharge>, HotDbException> PendingOther();
        public OneOf<Recharge, HotDbException> WebDuplicate(long AccessId, string Mobile, decimal Amount);
    }
}
