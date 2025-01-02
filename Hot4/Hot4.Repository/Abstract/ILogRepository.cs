using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface ILogRepository
    {
        Task AddLog(Log log);
        Task UpdateLog(Log log);
        Task DeleteLog(Log log);
        Task<List<LogModel>> ListLog(int pageNumber, int pageSize);
    }
}
