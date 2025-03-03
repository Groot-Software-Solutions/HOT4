using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Domain.Entities;

namespace Hot.Infrastructure.DbContext.Tables
{
    public class EconetReconZWLs : Table<EconetReconZWL>, IEconetReconZWL
    {
        public EconetReconZWLs(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x";
        }

        public OneOf<List<EconetReconZWL>, HotDbException> GetEconetZWLReconResult(DateTime startDate, DateTime EndDate)
        {
            throw new NotImplementedException();
        }

        public Task<OneOf<List<EconetReconZWL>, HotDbException>> GetEconetZWLReconResultAsync(DateTime startDate, DateTime EndDate)
        {
            throw new NotImplementedException();
        }
    } 
}
