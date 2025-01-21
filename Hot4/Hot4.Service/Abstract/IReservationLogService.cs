using Hot4.DataModel.Models;
using Hot4.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot4.Service.Abstract
{
    public interface IReservationLogService
    {
        Task<ReservationLogModel> GetReservationLogById(long ReservationLogId);
        Task<bool> AddReservationLog(ReservationLogModel  reservationLogModel);
        Task<bool> UpdateReservationLog(ReservationLogModel reservationLogModel);
        Task<bool> DeleteReservationLog(ReservationLogModel reservationLogModel);
        Task<List<ReservationLogModel>> ListReservationLog(int pageNo, int pageSize);
    }
}
