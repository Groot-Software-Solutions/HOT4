namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface IAddresses : IDbContextTable<Address>
         , IDbCanAdd<Address>
         , IDbCanAddInTransaction<Address>
         , IDbCanGetById<Address>
         , IDbCanUpdate<Address>
         , IDbCanUpdateInTransaction<Address>
    {
        public OneOf<List<Address>, HotDbException> SearchWithSageId(string sageId);
        public Task<OneOf<List<Address>, HotDbException>> SearchWithSageIdAsync(string sageId);
    }
}
