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
        public async Task AddReservationLog(ReservationLog reservationLog)
        {
            await Create(reservationLog);
            await SaveChanges();
        }
        public async Task DeleteReservationLog(ReservationLog reservationLog)
        {
            Delete(reservationLog);
            await SaveChanges();
        }
        public async Task<List<ReservationLogModel>> ListReservationLog(int pageNo, int pageSize)
        {
            return await PaginationFilter.GetPagedData(GetAll().OrderByDescending(d => d.ReservationLogId), pageNo, pageSize).Queryable
                 .Select(d => new ReservationLogModel
                 {
                     LastUser = d.LastUser,
                     LogDate = d.LogDate,
                     NewStateId = d.NewStateId,
                     OldStateId = d.OldStateId,
                     ReservationId = d.ReservationId,
                     ReservationLogId = d.ReservationLogId
                 }).ToListAsync();
        }
        public async Task UpdateReservationLog(ReservationLog reservationLog)
        {
            Update(reservationLog);
            await SaveChanges();
        }
    }
}
