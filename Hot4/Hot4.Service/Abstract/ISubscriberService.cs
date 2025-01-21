using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface ISubscriberService
    {
        Task<bool> AddSubscriber(SubscriberRecord subscriber);
        Task<bool> UpdateSubscriber(SubscriberRecord subscriber);
        Task<bool> DeleteSubscriber(long subscriberId);
        Task<SubscriberModel?> GetSubscriberById(long subscriberId);
        Task<List<SubscriberModel>> ListSubscriber(int pageNo, int pageSize);
    }
}
