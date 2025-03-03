namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface ILimits: IDbContextTable<Limit> 
        ,IDbCanAdd<Limit>  
    { 
        Task<OneOf<LimitModel, HotDbException>> GetCurrentLimitsAsync(int NetworkId, long AccountId);
    }
}
