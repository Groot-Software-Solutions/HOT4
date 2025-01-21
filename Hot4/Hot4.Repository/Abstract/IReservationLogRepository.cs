using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IReservationLogRepository
    {
        Task<ReservationLog> GetReservationLogById(long ReservationLogId);
        Task<bool> AddReservationLog(ReservationLog reservationLog);
        Task<bool> UpdateReservationLog(ReservationLog reservationLog);
        Task<bool> DeleteReservationLog(ReservationLog reservationLog);
        Task<List<ReservationLog>> ListReservationLog(int pageNo, int pageSize);
    }
}
