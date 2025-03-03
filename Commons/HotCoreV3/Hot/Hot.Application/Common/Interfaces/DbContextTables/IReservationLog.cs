namespace Hot.Application.Common.Interfaces.DbContextTables;

public interface IReservationLog:
    IDbCanAdd<ReservationLog>,
    IDbCanUpdate<ReservationLog>,
    IDbCanGetById<ReservationLog>
{

}