using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class SubscriberRepository : RepositoryBase<TblSubscriber>, ISubscriberRepository
    {
        public SubscriberRepository(HotDbContext context) : base(context) { }
        public async Task<TblSubscriber?> GetSubscriber(long subscriberId)
        {
            return await GetById(subscriberId);
        }

        public async Task InsertSubscriber(TblSubscriber subscriber)
        {
            await Create(subscriber);
            await SaveChanges();
        }

        public async Task<List<TblSubscriber>> ListSubscriber(long accountId)
        {
            return await GetByCondition(d => d.AccountId == accountId).ToListAsync();
        }

        public async Task UpdateSubscriber(TblSubscriber subscriber)
        {
            await Update(subscriber);
            await SaveChanges();
        }
    }
}
