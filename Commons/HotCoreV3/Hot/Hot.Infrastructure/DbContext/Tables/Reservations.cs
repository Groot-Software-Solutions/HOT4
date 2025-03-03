using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Domain.Entities;

namespace Hot.Infrastructure.DbContext.Tables;
public class Reservations : Table<Reservation>, IReservation
{
    public Reservations(IDbHelper dbHelper) : base(dbHelper)
    {
        base.StoredProcedurePrefix = "x";
        base.AddParameters = "@ReservationId, @AccessID, @RechargeID, @BrandId, @ProductCode, @NotificationNumber, @TargetNumber, @InsertDate, @StateId, @Amount, @Currency, @ConfirmationData";
        base.AddSuffix = "_Add";
        base.UpdateParameters = AddParameters;
        base.UpdateSuffix = AddSuffix;
        base.GetSuffix = "_Get";
    }

    public async Task<OneOf<Reservation, HotDbException>> GetByRechargeIdAsync(long rechargeID)
    {
        string query = $"xReservation_GetByRechargeId @RechargeId"; 
        var result = await dbHelper.QuerySingle<Reservation>(query, new { RechargeId = rechargeID});
        return result;
    }
}
