using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface ILogService
    {
        Task<bool> AddLog(LogModel log);
        Task<bool> UpdateLog(LogModel log);
        Task<bool> DeleteLog(long logId);
        Task<LogModel?> GetLogById(long logId);
        Task<List<LogModel>> ListLog(int pageNumber, int pageSize);
    }
}
