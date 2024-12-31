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

        public async Task<long> AddReservation(Reservation reservation)
        {
            await Create(reservation);
            await SaveChanges();
            return reservation.ReservationId;
        }

        public async Task DeleteReservation(Reservation reservation)
        {
            await Delete(reservation);
            await SaveChanges();
        }

        public async Task<List<ReservationModel>> GetReservationById(long reservationId)
        {
            return await GetByCondition(d => d.ReservationId == reservationId).Include(d => d.State)
                  .Select(d => new ReservationModel
                  {
                      ReservationId = d.ReservationId,
                      AccessId = d.AccessId,
                      Amount = d.Amount,
                      BrandId = d.BrandId,
                      ConfirmationData = d.ConfirmationData,
                      Currency = d.Currency,
                      InsertDate = d.InsertDate,
                      NotificationNumber = d.NotificationNumber,
                      ProductCode = d.ProductCode,
                      RechargeId = d.RechargeId,
                      StateId = d.StateId,
                      TargetNumber = d.TargetNumber,
                      ReservationState = d.State.ReservationState
                  }).ToListAsync();
        }

        public async Task<List<ReservationModel>> GetReservationByRechargeId(long rechargeId)
        {
            return await GetByCondition(d => d.RechargeId == rechargeId).Include(d => d.State)
                   .Select(d => new ReservationModel
                   {
                       ReservationId = d.ReservationId,
                       AccessId = d.AccessId,
                       Amount = d.Amount,
                       BrandId = d.BrandId,
                       ConfirmationData = d.ConfirmationData,
                       Currency = d.Currency,
                       InsertDate = d.InsertDate,
                       NotificationNumber = d.NotificationNumber,
                       ProductCode = d.ProductCode,
                       RechargeId = d.RechargeId,
                       StateId = d.StateId,
                       TargetNumber = d.TargetNumber,
                       ReservationState = d.State.ReservationState
                   }).ToListAsync();
        }

        public async Task UpdateReservation(Reservation reservation)
        {
            await Update(reservation);
            await SaveChanges();
        }
    }
}
