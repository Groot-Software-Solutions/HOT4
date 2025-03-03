namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface IAccounts : 
        IDbContextTable<Account>,
        IDbCanAdd<Account>,
        IDbCanAddInTransaction<Account>, 
        IDbCanGetById<Account>,
        IDbCanSearch<Account>, 
        IDbCanUpdate<Account>, 
        IDbCanUpdateInTransaction<Account>
    { 
        [Obsolete(message:"Please use Get(Id) function instead" )]
        public Task<OneOf<Account, HotDbException>> SelectRow(long accountId);
        
    }
}
