namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface IAccesss : IDbContextTable<Access>
        , IDbCanAdd<Access>
        , IDbCanAddInTransaction<Access>
        , IDbCanGetById<Access>
        , IDbCanList<Access>
        , IDbCanRemoveById<Access>
        , IDbCanSearch<Access>
        , IDbCanUpdate<Access>
        , IDbCanUpdateInTransaction<Access>
    { 
        public Task<OneOf<bool, HotDbException>> PasswordChangeAsync(Access access);
        public OneOf<bool, HotDbException> PasswordChange(Access access);

        public Task<OneOf<long, HotDbException>> AdminSelectAsync(long accountId);
        public OneOf<long, HotDbException> AdminSelect(long accountId);

        public Task<OneOf<Access, HotDbException>> SelectCodeAsync(string accessCode);
        public OneOf<Access, HotDbException> SelectCode(string accessCode);

        public Task<OneOf<Access, HotDbException>> SelectLoginAsync(string accessCode,string accessPassword);
        public OneOf<Access, HotDbException> SelectLogin(string accessCode, string accessPassword);
        
        public Task<OneOf<Access, HotDbException>> SelectRowAsync(long accessId);
        public OneOf<Access, HotDbException> SelectRow(long accessId);
        
        public Task<OneOf<bool, HotDbException>> UnDeleteAsync(long accessId);
        public OneOf<bool, HotDbException> UnDelete(long accessId); 
        
        public Task<OneOf<List<Access>, HotDbException>> ListDeletedAsync(long accountId);
        public OneOf<List<Access>, HotDbException> ListDeleted(long accountId);
        
        public Task<OneOf<List<Access>, HotDbException>> ListAsync(long AccountId);
        public OneOf<List<Access>, HotDbException> List(long AccountId);
         
        public Task<OneOf<List<Access>, HotDbException>> ListActiveAsync(long accountId);
        public OneOf<List<Access>, HotDbException> ListActive(long AccountId);
    }
}
