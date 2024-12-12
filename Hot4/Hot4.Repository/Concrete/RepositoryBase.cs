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


        public async Task Update(T entity)
        {
            this._context.Set<T>().Update(entity);
        }
        public async Task Delete(T entity)
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

        public async Task<object?> CreateReturn(T entity, string fieldname)
        {

            var res = await this._context.Set<T>().AddAsync(entity);
            if (res != null)
            {
                var idProperty = res.GetType().GetProperty(fieldname)?.GetValue(entity, null);
                return (object?)idProperty;
            }
            else
            {
                return 0;
            }
        }

        public async Task BulkUpdate(List<T> entities)
        {
            this._context.UpdateRange(entities);
        }
        //public void BulkDelete(List<T> entities)
        //{
        //    this._context.RemoveRange(entities);
        //}
    }
}
