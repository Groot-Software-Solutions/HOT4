using AutoMapper;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class StatisticsService : IStatisticsService
    {
        private IStatisticsRepository _statisticsRepository;
        private readonly IMapper Mapper;
        public StatisticsService(IStatisticsRepository statisticsRepository, IMapper mapper)
        {
            _statisticsRepository = statisticsRepository;
            Mapper = mapper;
        }

        public async Task<List<StatisticsTrafficModel>> GetRechargeTrafficDayStatistics()
        {
            return await _statisticsRepository.GetRechargeTrafficDayStatistics();
        }

        public async Task<List<StatisticsTrafficModel>> GetRechargeTrafficStatistics()
        {
            return await _statisticsRepository.GetRechargeTrafficStatistics();
        }

        public async Task<List<StatisticsTrafficModel>> GetSMSTrafficStatistics()
        {
            return await _statisticsRepository.GetSMSTrafficStatistics();
        }
    }
}
