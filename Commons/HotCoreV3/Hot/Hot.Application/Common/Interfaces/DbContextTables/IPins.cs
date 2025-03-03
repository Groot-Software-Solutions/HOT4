namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface IPins : IDbContextTable<Pin>
        , IDbCanAdd<Pin>
        ,IDbCanAddInTransaction<Pin>
    {
        public Task<OneOf<List<Pin>, HotDbException>> PinsInBatchAsync(int PinBatchId);
        public OneOf<List<Pin>, HotDbException> PinsInBatch(int PinBatchId);
       
        public Task<OneOf<List<PinStockModel>, HotDbException>> StockAsync();
        public OneOf<List<PinStockModel>, HotDbException> Stock();
         
        public Task<OneOf<List<PinStockModel>, HotDbException>> StockLoadedInBatchAsync(int PinBatchId);
        public OneOf<List<PinStockModel>, HotDbException> StockLoadedInBatch(int PinBatchId);
         
        public Task<OneOf<List<PinStockModel>, HotDbException>> PromoStockAsync();
        public OneOf<List<PinStockModel>, HotDbException> PromoStock();

        public OneOf<List<Pin>, HotDbException> PromoRecharge(string AccessCode, int BrandId, decimal PinValue, int Quantity, string Mobile);
        public Task<OneOf<List<Pin>, HotDbException>> PromoRechargeAsync(string AccessCode, int BrandId, decimal PinValue, int Quantity, string Mobile);
        
        public Task<OneOf<bool, HotDbException>> PromoHasPurchasedAsync(long AccountId);
        public OneOf<bool, HotDbException> PromoHasPurchased(long AccountId);

        public OneOf<List<Pin>, HotDbException> Recharge(decimal Amount, int BrandId, long RechargeId);
        public Task<OneOf<List<Pin>, HotDbException>> RechargeAsync(decimal Amount, int BrandId, long RechargeId);

    }

}
