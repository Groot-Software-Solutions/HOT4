using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class ReservationStateRepository : RepositoryBase<ReservationStates>, IReservationStateRepository
    {
        public ReservationStateRepository(HotDbContext context) : base(context) { }
        public async Task<bool> AddReservationState(ReservationStates reservationState)
        {
            await Create(reservationState);
            await SaveChanges();
            return true;
        }
        public async Task<bool> DeleteReservationState(ReservationStates reservationState)
        {
            Delete(reservationState);
            await SaveChanges();
            return true;
        }

        public async Task<ReservationStates?> GetReservationStateById(byte ReservationStateId)
        {
            return await GetById(ReservationStateId);
        }

        public async Task<List<ReservationStates>> ListReservationState()
        {
            return await GetAll().ToListAsync();
        }
        public async Task<bool> UpdateReservationState(ReservationStates reservationState)
        {
            Update(reservationState);
            await SaveChanges();
            return true;
        }
    }
}
