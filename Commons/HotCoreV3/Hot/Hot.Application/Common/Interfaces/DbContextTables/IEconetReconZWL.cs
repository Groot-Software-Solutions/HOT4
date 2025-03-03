namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface IEconetReconZWL : IDbContextTable<EconetReconZWL>
     , IDbCanAdd<EconetReconZWL>
    {
        public Task<OneOf<List<EconetReconZWL>, HotDbException>> GetEconetZWLReconResultAsync(DateTime startDate, DateTime EndDate);
        public OneOf<List<EconetReconZWL>, HotDbException> GetEconetZWLReconResult(DateTime startDate, DateTime EndDate);
    }
}
