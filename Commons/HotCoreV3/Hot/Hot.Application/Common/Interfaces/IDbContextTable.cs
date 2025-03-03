using System.Data;

namespace Hot.Application.Common.Interfaces
{
    public interface IDbContextTable<T>
    { 
        
    }

    public interface IDbCanAdd<T>
    {
        public Task<OneOf<int, HotDbException>> AddAsync(T Item);
        public OneOf<int, HotDbException> Add(T Item);

    }

    public interface IDbCanAddInTransaction<T>
    {
        public Task<OneOf<int, HotDbException>> AddAsync(T Item, IDbConnection connection, IDbTransaction transaction);
        public OneOf<int, HotDbException> Add(T Item, IDbConnection connection, IDbTransaction transaction);

    }

    public interface IDbCanGetById<T>
    {
        public Task<OneOf<T, HotDbException>> GetAsync(int Id);
        public OneOf<T, HotDbException> Get(int Id);
        public Task<OneOf<T, HotDbException>> GetAsync(long Id);
        public OneOf<T, HotDbException> Get(long Id);
    }

    public interface IDbCanSearch<T>
    {
        public Task<OneOf<List<T>, HotDbException>> SearchAsync(string Id);
        public OneOf<List<T>, HotDbException> Search(string Id);

    }

    public interface IDbCanList<T> 
    {
        public Task<OneOf<List<T>, HotDbException>> ListAsync();
        public OneOf<List<T>, HotDbException> List();
    }
     
    public interface IDbCanRemoveById<T> 
    {
        public Task<OneOf<bool, HotDbException>> RemoveAsync(int Id);
        public OneOf<bool, HotDbException> Remove(int Id);
    }

    public interface IDbCanUpdate<T> 
    {
        public Task<OneOf<bool, HotDbException>> UpdateAsync(T Item);
        public OneOf<bool, HotDbException> Update(T Item);
    }

    public interface IDbCanUpdateInTransaction<T> 
    {
        public Task<OneOf<bool, HotDbException>> UpdateAsync(T Item, IDbConnection connection, IDbTransaction transaction);
        public OneOf<bool, HotDbException> Update(T Item, IDbConnection connection, IDbTransaction transaction);
    }

}
