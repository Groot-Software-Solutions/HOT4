using Hot4.DataModel.Models;
using Hot4.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot4.Service.Abstract
{
    public interface IReservationService
    {
        Task<bool> AddReservation(ReservationModel reservationModel);
        Task<bool> UpdateReservation(ReservationModel reservationModel);
        Task<ReservationModel> GetReservationById(long reservationId);
        Task<List<ReservationModel>> GetReservationByRechargeId(long rechargeId);
        Task<bool> DeleteReservation(long reservationId);
    }
}
