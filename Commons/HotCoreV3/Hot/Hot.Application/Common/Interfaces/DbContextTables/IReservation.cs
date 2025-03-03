 
namespace Hot.Application.Common.Interfaces.DbContextTables;
public interface IReservation :
    IDbCanAdd<Reservation>,
    IDbCanUpdate<Reservation>,
    IDbCanGetById<Reservation>
{
    public Task<OneOf<Reservation,HotDbException>> GetByRechargeIdAsync(long rechargeID);
}
