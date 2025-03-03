namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface ITelecelReconZWL : IDbContextTable<TelecelReconZWL>
     , IDbCanAdd<TelecelReconZWL>
    {
        public Task<OneOf<List<TelecelReconZWL>, HotDbException>> GetTelecelZWLReconResultAsync(DateTime startDate, DateTime EndDate);
        public OneOf<List<TelecelReconZWL>, HotDbException> GetTelecelZWLReconResult(DateTime startDate, DateTime EndDate);
    }
}
