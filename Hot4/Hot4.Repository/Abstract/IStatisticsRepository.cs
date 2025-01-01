using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IStatisticsRepository
    {
        Task<List<StatisticsTrafficModel>> GetRechargeTrafficStatistics();
        Task<List<StatisticsTrafficModel>> GetRechargeTrafficDayStatistics();
        Task<List<StatisticsTrafficModel>> GetSMSTrafficStatistics();
    }
}
