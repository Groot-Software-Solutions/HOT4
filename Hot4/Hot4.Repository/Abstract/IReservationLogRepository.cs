using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IReservationLogRepository
    {
        Task AddReservationLog(ReservationLog reservationLog);
        Task UpdateReservationLog(ReservationLog reservationLog);
        Task DeleteReservationLog(ReservationLog reservationLog);
        Task<List<ReservationLogModel>> ListReservationLog(int pageNo, int pageSize);
    }
}
