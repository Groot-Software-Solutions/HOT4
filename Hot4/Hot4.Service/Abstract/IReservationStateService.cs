using Hot4.DataModel.Models;
using Hot4.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot4.Service.Abstract
{
    public interface IReservationStateService
    {
        Task<bool> AddReservationState(ReservationStateModel reservationStateModel);
        Task<bool> UpdateReservationState(ReservationStateModel reservationStateModel);
        Task<bool> DeleteReservationState(ReservationStateModel reservationStateModel);
        Task<List<ReservationStateModel>> ListReservationState();
        Task<ReservationStateModel> GetReservationStateById(byte ReservationStateId);
    }
}
