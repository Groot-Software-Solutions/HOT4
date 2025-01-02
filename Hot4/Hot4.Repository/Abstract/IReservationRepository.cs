using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IReservationRepository
    {
        Task<long> AddReservation(Reservation reservation);
        Task UpdateReservation(Reservation reservation);
        Task<List<ReservationModel>> GetReservationById(long reservationId);
        Task<List<ReservationModel>> GetReservationByRechargeId(long rechargeId);
        Task DeleteReservation(Reservation reservation);
    }
}
