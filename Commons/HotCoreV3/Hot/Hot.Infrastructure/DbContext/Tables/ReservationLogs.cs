using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Domain.Entities;

namespace Hot.Infrastructure.DbContext.Tables;

public class ReservationLogs : Table<ReservationLog>, IReservationLog
{
    public ReservationLogs(IDbHelper dbHelper) : base(dbHelper)
    {
    }
}