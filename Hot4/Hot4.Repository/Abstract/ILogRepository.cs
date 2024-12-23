using Hot4.DataModel.Models;
using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface ILogRepository
    {
        public Task AddLog(Log log);
        public Task<List<LogModel>> GetAll(int pageNumber, int pageSize);
    }
}
