using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface ISubscriberRepository
    {
        Task AddSubscriber(Subscriber subscriber);
        Task UpdateSubscriber(Subscriber subscriber);
        Task DeleteSubscriber(Subscriber subscriber);
        Task<SubscriberModel?> GetSubscriberById(long subscriberId);
        Task<List<SubscriberModel>> ListSubscriber(int pageNo, int pageSize);
    }
}
