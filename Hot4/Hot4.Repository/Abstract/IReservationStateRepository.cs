using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IReservationStateRepository
    {
        Task <bool>AddReservationState(ReservationStates reservationState);
        Task<bool> UpdateReservationState(ReservationStates reservationState);
        Task <bool>DeleteReservationState(ReservationStates reservationState);
        Task<List<ReservationStates>> ListReservationState();
        Task<ReservationStates> GetReservationStateById(byte ReservationStateId);
    }
}
