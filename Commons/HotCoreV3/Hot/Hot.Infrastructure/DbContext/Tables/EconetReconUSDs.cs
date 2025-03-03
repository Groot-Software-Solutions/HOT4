using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Domain.Entities;

namespace Hot.Infrastructure.DbContext.Tables
{
    public class EconetReconUSDs : Table<EconetReconUSD>, IEconetReconUSD
    {
        public EconetReconUSDs(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x";
            base.AddSuffix = "_Save";
            base.AddParameters = "@startDate,@endDate";
        }

        public OneOf<List<EconetReconUSD>, HotDbException> GetEconetUSDReconResult(DateTime startDate, DateTime endDate)
        {
           return GetEconetUSDReconResultAsync(startDate, endDate).Result;  
        }

        public async Task<OneOf<List<EconetReconUSD>, HotDbException>> GetEconetUSDReconResultAsync(DateTime startDate, DateTime endDate)
        {
            string query = $"{GetSPPrefix()}_Results {AddParameters}";
            var result = await dbHelper.Query<EconetReconUSD>(query,new { startDate, endDate });
            //var result = await dbHelper.ExecuteScalar<List<EconetReconUSD>,object>(query,new { startDate, endDate });
            //var result = await dbHelper.Query<EconetReconUSD>($"{GetSPPrefix()}_Results", new { startDate, EndDate });
            return result;
        }

    }
}
