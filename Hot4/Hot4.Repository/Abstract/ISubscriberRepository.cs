using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface ISubscriberRepository
    {
        Task<List<Subscriber>> ListSubscriber(long accountId);

        Task<Subscriber?> GetSubscriber(long subscriberId);
        Task InsertSubscriber(Subscriber subscriber);
        Task UpdateSubscriber(Subscriber subscriber);
    }
}
