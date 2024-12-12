using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface ISubscriberRepository
    {
        Task<List<TblSubscriber>> ListSubscriber(long accountId);

        Task<TblSubscriber?> GetSubscriber(long subscriberId);
        Task InsertSubscriber(TblSubscriber subscriber);
        Task UpdateSubscriber(TblSubscriber subscriber);
    }
}
