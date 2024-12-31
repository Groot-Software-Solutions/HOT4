using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IReservationStateRepository
    {
        Task AddReservationState(ReservationStates reservationState);
        Task UpdateReservationState(ReservationStates reservationState);
        Task DeleteReservationState(ReservationStates reservationState);
        Task<List<ReservationStateModel>> GetAllReservationState();
    }
}
