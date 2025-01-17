using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface ISubscriberRepository
    {
        Task<bool> AddSubscriber(Subscriber subscriber);
        Task<bool> UpdateSubscriber(Subscriber subscriber);
        Task<bool> DeleteSubscriber(Subscriber subscriber);
        Task<Subscriber?> GetSubscriberById(long subscriberId);
        Task<List<Subscriber>> ListSubscriber(int pageNo, int pageSize);
    }
}
