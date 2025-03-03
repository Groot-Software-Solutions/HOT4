namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface IRechargePrepaids : IDbContextTable<RechargePrepaid>
        , IDbCanAdd<RechargePrepaid>
        , IDbCanUpdate<RechargePrepaid>
        , IDbCanGetById<RechargePrepaid>
    { 
    }
}
