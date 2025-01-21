using Hot4.Core.Helper;
using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class LogRepository : RepositoryBase<Log>, ILogRepository
    {
        public LogRepository(HotDbContext context) : base(context) { }
        public async Task<bool> AddLog(Log log)
        {
            log.LogDate = DateTime.Now;
            await Create(log);
            await SaveChanges();
            return true;
        }
        public async Task<bool> UpdateLog(Log log)
        {
            Update(log);
            await SaveChanges();
            return true;
        }
        public Task<Log?> GetLogById(long logId)
        {
            return GetById(logId);
        }
        public async Task<bool> DeleteLog(Log log)
        {
            Delete(log);
            await SaveChanges();
            return true;
        }
        public async Task<List<Log>> ListLog(int pageNo, int pageSize)
        {
            return await PaginationFilter.GetPagedData(GetAll().OrderByDescending(d => d.LogId), pageNo, pageSize).Queryable
                              .ToListAsync();
        }


    }
}
