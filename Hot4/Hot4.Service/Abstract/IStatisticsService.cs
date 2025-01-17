using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface IStatisticsService
    {
        Task<List<StatisticsTrafficModel>> GetRechargeTrafficStatistics();
        Task<List<StatisticsTrafficModel>> GetRechargeTrafficDayStatistics();
        Task<List<StatisticsTrafficModel>> GetSMSTrafficStatistics();
    }
}
