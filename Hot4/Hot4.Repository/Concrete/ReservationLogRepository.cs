using Hot4.Core.Helper;
using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class ReservationLogRepository : RepositoryBase<ReservationLog>, IReservationLogRepository
    {
        public ReservationLogRepository(HotDbContext context) : base(context) { }
        public async Task<bool> AddReservationLog(ReservationLog reservationLog)
        {
            await Create(reservationLog);
            await SaveChanges();
            return true;
        }
        public async Task<bool> DeleteReservationLog(ReservationLog reservationLog)
        {
            Delete(reservationLog);
            await SaveChanges();
            return true;
        }
        public async Task<ReservationLog?> GetReservationLogById(long ReservationLogId)
        {
            return await GetById(ReservationLogId);
        }
        public async Task<List<ReservationLog>> ListReservationLog(int pageNo, int pageSize)
        {
            return await PaginationFilter.GetPagedData(GetAll().OrderByDescending(d => d.ReservationLogId), pageNo, pageSize).Queryable
                 .ToListAsync();
        }

        public async Task<bool> UpdateReservationLog(ReservationLog reservationLog)
        {
            Update(reservationLog);
            await SaveChanges();
            return true;
        }
    }
}
