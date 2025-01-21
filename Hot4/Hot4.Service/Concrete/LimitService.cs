using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class LimitService : ILimitService
    {
        private readonly ILimitRepository _limitRepository;
        private readonly IMapper Mapper;
        public LimitService(ILimitRepository limitRepository, IMapper mapper)
        {
            _limitRepository = limitRepository;
            Mapper = mapper;
        }
        public async Task<bool> DeleteLimit(long LimitId)
        {
            var record = await GetEntityById(LimitId);
            if (record != null)
            {
                return await _limitRepository.DeleteLimit(record);
            }
            return false;
        }
        public async Task<LimitModel?> GetLimitById(long limitId)
        {
            var record = await GetEntityById(limitId);
            return Mapper.Map<LimitModel?>(record);
        }
        public async Task<LimitPendingModel?> GetLimitByNetworkAndAccountId(int networkid, long accountid)
        {
            var record = await _limitRepository.GetLimitByNetworkAndAccountId(networkid, accountid);
            if (record != null)
            {
                float remainingMonthlyLimit = (record.MonthlyLimit ?? 10000) - (record.SalesMonthly ?? 0);
                float remainingDailyLimit = (record.DailyLimit ?? 1000) - (record.SalesToday ?? 0);

                float remainingLimit = remainingDailyLimit;
                if ((record.MonthlyLimit ?? 10000) - (record.SalesMonthly ?? 0) < remainingLimit)
                {
                    remainingLimit = (record.MonthlyLimit ?? 10000) - (record.SalesMonthly ?? 0);
                }
                record.LimitRemaining = remainingLimit;
                record.RemainingLimit = remainingLimit;
                record.RemainingDailyLimit = remainingDailyLimit;
                record.RemainingMonthlyLimit = remainingMonthlyLimit;
                record.DailyLimit = record.DailyLimit ?? 1000;
                record.MonthlyLimit = record.MonthlyLimit ?? 10000;

            }
            return record;
        }
        public async Task<bool> SaveLimit(LimitModel limit)
        {
            if (limit != null)
            {
                var model = Mapper.Map<Limit>(limit);
                return await _limitRepository.SaveLimit(model);
            }
            return false;
        }
        public async Task<bool> UpdateLimit(LimitModel limit)
        {
            var record = await GetEntityById(limit.LimitId);
            if (record != null)
            {
                var model = Mapper.Map(limit, record);
                return await _limitRepository.UpdateLimit(model);
            }
            return false;
        }
        private async Task<Limit?> GetEntityById(long limitId)
        {
            return await _limitRepository.GetLimitById(limitId);
        }
    }
}
