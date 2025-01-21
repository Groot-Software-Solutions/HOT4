using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;
        private readonly IMapper Mapper;
        public LogService(ILogRepository logRepository, IMapper mapper)
        {
            _logRepository = logRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddLog(LogModel log)
        {
            if (log != null)
            {
                var model = Mapper.Map<Log>(log);
                return await _logRepository.AddLog(model);
            } 
            return false;
        }
        public async Task<bool> DeleteLog(long logId)
        {
            var record = await GetEntityById(logId);
            if (record != null)
            {
                return await _logRepository.DeleteLog(record);
            }
            return false;
        }
        public async Task<LogModel?> GetLogById(long logId)
        {
            var record = await GetEntityById(logId);
            return Mapper.Map<LogModel?>(record);
        }
        public async Task<List<LogModel>> ListLog(int pageNumber, int pageSize)
        {
            var records = await _logRepository.ListLog(pageNumber, pageSize);
            return Mapper.Map<List<LogModel>>(records);
        }
        public async Task<bool> UpdateLog(LogModel log)
        {
            var record = await GetEntityById(log.LogId);
            if (record != null)
            {
                var model = Mapper.Map(log, record);
                return await _logRepository.UpdateLog(record);
            }
            return false;
        }
        private async Task<Log?> GetEntityById(long logId)
        {
            return await _logRepository.GetLogById(logId);
        }
    }
}
