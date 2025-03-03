namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface IEconetReconUSD : IDbContextTable<EconetReconUSD>
 , IDbCanAdd<EconetReconUSD>
    {
        public Task<OneOf<List<EconetReconUSD>, HotDbException>> GetEconetUSDReconResultAsync(DateTime startDate, DateTime endDate);
        public OneOf<List<EconetReconUSD>, HotDbException> GetEconetUSDReconResult(DateTime startDate, DateTime endDate);
    }

}
