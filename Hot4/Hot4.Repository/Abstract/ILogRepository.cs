using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface ILogRepository
    {
        Task<bool> AddLog(Log log);
        Task<bool> UpdateLog(Log log);
        Task<bool> DeleteLog(Log log);
        Task<Log?> GetLogById(long logId);
        Task<List<Log>> ListLog(int pageNumber, int pageSize);
    }
}
