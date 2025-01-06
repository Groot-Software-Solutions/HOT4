using Hot4.Core.Helper;
using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class LogRepository : RepositoryBase<Log>, ILogRepository
    {
        public LogRepository(HotDbContext context) : base(context) { }
        public async Task AddLog(Log log)
        {
            log.LogDate = DateTime.Now;
            await Create(log);
            await SaveChanges();
        }
        public async Task UpdateLog(Log log)
        {
            Update(log);
            await SaveChanges();
        }
        public async Task DeleteLog(Log log)
        {
            Delete(log);
            await SaveChanges();
        }
        public async Task<List<LogModel>> ListLog(int pageNo, int pageSize)
        {
            return await PaginationFilter.GetPagedData(GetAll().OrderByDescending(d => d.LogId), pageNo, pageSize).Queryable
                               .Select(d => new LogModel
                               {
                                   LogDate = d.LogDate,
                                   LogDescription = d.LogDescription,
                                   LogMethod = d.LogMethod,
                                   LogModule = d.LogModule,
                                   LogObject = d.LogObject,
                                   Idnumber = d.Idnumber,
                                   Idtype = d.Idtype,
                                   LogId = d.LogId
                               }).ToListAsync();
        }

    }
}
