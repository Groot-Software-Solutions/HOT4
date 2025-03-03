using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Domain.Entities;

namespace Hot.Infrastructure.DbContext.Tables
{
    public class TelecelReconZWLs : Table<TelecelReconZWL>, ITelecelReconZWL
    {
        public TelecelReconZWLs(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x";
        }

        public OneOf<List<TelecelReconZWL>, HotDbException> GetTelecelZWLReconResult(DateTime startDate, DateTime EndDate)
        {
            throw new NotImplementedException();
        }

        public Task<OneOf<List<TelecelReconZWL>, HotDbException>> GetTelecelZWLReconResultAsync(DateTime startDate, DateTime EndDate)
        {
            throw new NotImplementedException();
        }
    }
}
