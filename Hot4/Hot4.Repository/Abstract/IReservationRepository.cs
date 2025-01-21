using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IReservationRepository
    {
        Task<bool> AddReservation(Reservation reservation);
        Task<bool> UpdateReservation(Reservation reservation);
        Task<Reservation> GetReservationById(long reservationId);
        Task<List<Reservation>> GetReservationByRechargeId(long rechargeId);
        Task<bool> DeleteReservation(Reservation reservation);
    }
}
