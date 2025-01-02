using System.Linq.Expressions;

namespace Hot4.Repository.Abstract
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> GetAll();
        Task<T?> GetById(object id);
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression);
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task SaveChanges();
        Task BulkCreate(List<T> entities);

        Task BulkUpdate(List<T> entities);
    }
}
