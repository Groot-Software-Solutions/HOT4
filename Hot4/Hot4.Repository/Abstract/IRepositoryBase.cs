using System.Linq.Expressions;

namespace Hot4.Repository.Abstract
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> GetAll();
        Task<T?> GetById(object id);
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression);
        Task Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveChanges();
        Task BulkCreate(List<T> entities);

        void BulkUpdate(List<T> entities);
    }
}
