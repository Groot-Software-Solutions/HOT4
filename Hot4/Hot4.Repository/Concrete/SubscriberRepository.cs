using Hot4.Core.Helper;
using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class SubscriberRepository : RepositoryBase<Subscriber>, ISubscriberRepository
    {
        public SubscriberRepository(HotDbContext context) : base(context) { }
        public async Task AddSubscriber(Subscriber subscriber)
        {
            await Create(subscriber);
            await SaveChanges();
        }

        public async Task DeleteSubscriber(Subscriber subscriber)
        {
            Delete(subscriber);
            await SaveChanges();
        }
        public async Task UpdateSubscriber(Subscriber subscriber)
        {
            Update(subscriber);
            await SaveChanges();
        }
        public async Task<SubscriberModel?> GetSubscriberById(long subscriberId)
        {
            var result = await _context.Subscriber.Include(d => d.Brand)
                .FirstOrDefaultAsync(d => d.SubscriberId == subscriberId);
            if (result != null)
            {
                return new SubscriberModel
                {
                    SubscriberId = result.SubscriberId,
                    AccountId = result.AccountId,
                    Active = result.Active,
                    BrandId = result.BrandId,
                    BrandName = result.Brand.BrandName,
                    DefaultAmount = result.DefaultAmount,
                    DefaultProductId = result.DefaultProductId,
                    NetworkId = result.NetworkId,
                    NotifyNumber = result.NotifyNumber,
                    SubscriberGroup = result.SubscriberGroup,
                    SubscriberMobile = result.SubscriberMobile,
                    SubscriberName = result.SubscriberName,
                };
            }
            return null;
        }

        public async Task<List<SubscriberModel>> ListSubscriber(int pageNo, int pageSize)
        {
            return await PaginationFilter.GetPagedData(GetAll().Include(d => d.Brand)
                .OrderByDescending(d => d.SubscriberId), pageNo, pageSize).Queryable
                .Select(d => new SubscriberModel
                {
                    SubscriberId = d.SubscriberId,
                    AccountId = d.AccountId,
                    Active = d.Active,
                    BrandId = d.BrandId,
                    BrandName = d.Brand.BrandName,
                    DefaultAmount = d.DefaultAmount,
                    DefaultProductId = d.DefaultProductId,
                    NetworkId = d.NetworkId,
                    NotifyNumber = d.NotifyNumber,
                    SubscriberGroup = d.SubscriberGroup,
                    SubscriberMobile = d.SubscriberMobile,
                    SubscriberName = d.SubscriberName,
                }).ToListAsync();
        }
    }
}
