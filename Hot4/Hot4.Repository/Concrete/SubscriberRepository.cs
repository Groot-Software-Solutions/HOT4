using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class SubscriberRepository : RepositoryBase<Subscriber>, ISubscriberRepository
    {
        public SubscriberRepository(HotDbContext context) : base(context) { }
        public async Task<Subscriber?> GetSubscriber(long subscriberId)
        {
            return await GetById(subscriberId);
        }

        public async Task InsertSubscriber(Subscriber subscriber)
        {
            await Create(subscriber);
            await SaveChanges();
        }

        public async Task<List<Subscriber>> ListSubscriber(long accountId)
        {
            return await GetByCondition(d => d.AccountId == accountId).ToListAsync();
        }

        public async Task UpdateSubscriber(Subscriber subscriber)
        {
            await Update(subscriber);
            await SaveChanges();
        }
    }
}
