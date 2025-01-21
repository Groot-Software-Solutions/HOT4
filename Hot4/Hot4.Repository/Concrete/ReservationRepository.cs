using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class ReservationRepository : RepositoryBase<Reservation>, IReservationRepository
    {
        public ReservationRepository(HotDbContext context) : base(context) { }

        public async Task<bool> AddReservation(Reservation reservation)
        {
            await Create(reservation);
            await SaveChanges();
            return true;
        }
        public async Task<bool> DeleteReservation(Reservation reservation)
        {
            Delete(reservation);
            await SaveChanges();
            return true;
        }
        public async Task<Reservation?> GetReservationById(long reservationId)
        {
            return await GetByCondition(d => d.ReservationId == reservationId).Include(d => d.State).FirstOrDefaultAsync();
                  
        }
        public async Task<List<Reservation>> GetReservationByRechargeId(long rechargeId)
        {
            return await GetByCondition(d => d.RechargeId == rechargeId).Include(d => d.State)
                   .ToListAsync();
        }
        public async Task<bool> UpdateReservation(Reservation reservation)
        {
            Update(reservation);
            await SaveChanges();
            return true;
        }
    }
}
