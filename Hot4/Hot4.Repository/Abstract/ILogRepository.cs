using Hot4.DataModel.Models;
using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface ILogRepository
    {
        Task AddLog(Log log);
        Task<List<LogModel>> GetAll(int pageNumber, int pageSize);
    }
}
