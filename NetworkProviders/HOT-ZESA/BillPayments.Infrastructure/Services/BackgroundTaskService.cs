using BillPayments.Infrastructure.Data;
using BillPayments.Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BillPayments.Application.Services;

namespace BillPayments.Infrastructure.Services
{
    public class BackgroundTaskService : IBackgroundTaskService
    {
        private readonly AppDbContext _dbContext;

        public BackgroundTaskService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task SaveTask(BackgroundTask task)
        {
            if (task.Id == 0)
            {
                _dbContext.BackgroundTasks.Add(task);
            }
            else
            {
                var dbTask = await _dbContext.BackgroundTasks.SingleOrDefaultAsync(x => x.Id == task.Id);
                if (dbTask != null)
                {
                    dbTask.EntityBody = task.EntityBody;
                    dbTask.NumberOfRetries = task.NumberOfRetries + 1;
                    dbTask.DateOfLastRetry = task.DateOfLastRetry;
                    dbTask.RetrySucceeded = task.RetrySucceeded;
                }
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<BackgroundTask>> GetUnprocessedTasks()
        {
            var tasks = await _dbContext.BackgroundTasks.Where(x => x.RetrySucceeded == false).ToListAsync();
            return tasks;
        }

    }
}
