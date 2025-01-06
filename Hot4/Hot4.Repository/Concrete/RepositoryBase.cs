using Hot4.DataModel.Data;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hot4.Repository.Concrete
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected HotDbContext _context { get; set; }

        public RepositoryBase(HotDbContext hotDbContext)
        {
            _context = hotDbContext;
        }
        public IQueryable<T> GetAll()
        {
            return this._context.Set<T>().AsNoTracking();
        }
        public async Task<T?> GetById(object id)
        {
            return await this._context.Set<T>().FindAsync(id);
        }
        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return this._context.Set<T>()
                .Where(expression).AsNoTracking();
        }
        public async Task Create(T entity)
        {

            await this._context.Set<T>().AddAsync(entity);

        }
        public void Update(T entity)
        {
            this._context.Set<T>().Update(entity);
        }
        public void Delete(T entity)
        {
            this._context.Set<T>().Remove(entity);
        }
        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
        public async Task BulkCreate(List<T> entities)
        {
            await this._context.Set<List<T>>().AddRangeAsync(entities);
        }

        public void BulkUpdate(List<T> entities)
        {
            this._context.UpdateRange(entities);
        }

    }
}
