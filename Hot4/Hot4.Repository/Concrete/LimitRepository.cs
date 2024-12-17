using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;

namespace Hot4.Repository.Concrete
{
    public class LimitRepository : RepositoryBase<Limit>, ILimitRepository
    {
        public LimitRepository(HotDbContext context) : base(context)
        {

        }
        public async Task<long> AddLimit(Limit limit)
        {
            await Create(limit);
            await SaveChanges();
            return limit.LimitId;
        }

        public async Task DeleteLimit(long limitId)
        {
            var limit = await GetLimit(limitId);
            if (limit != null)
            {
                await Delete(limit);
                await SaveChanges();
            }
        }

        public async Task<Limit?> GetLimit(long limitId)
        {
            return await GetById(limitId);
        }

        public async Task UpdateLimit(Limit limit)
        {
            await Update(limit);
            await SaveChanges();
        }
    }
}
