using BillPayments.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BillPayments.Application.Services
{
    public interface IBackgroundTaskService
    {
        Task<IEnumerable<BackgroundTask>> GetUnprocessedTasks();
        Task SaveTask(BackgroundTask task);
    }
}