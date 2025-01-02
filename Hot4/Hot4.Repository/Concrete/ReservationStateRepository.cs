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
        public async Task AddReservationState(ReservationStates reservationState)
        {
            await Create(reservationState);
            await SaveChanges();
        }
        public async Task DeleteReservationState(ReservationStates reservationState)
        {
            await Delete(reservationState);
            await SaveChanges();
        }
        public async Task<List<ReservationStateModel>> ListReservationState()
        {
            return await GetAll().Select(d => new ReservationStateModel
            {
                ReservationStateId = d.ReservationStateId,
                ReservationState = d.ReservationState
            }).ToListAsync();
        }
        public async Task UpdateReservationState(ReservationStates reservationState)
        {
            await Update(reservationState);
            await SaveChanges();
        }
    }
}
