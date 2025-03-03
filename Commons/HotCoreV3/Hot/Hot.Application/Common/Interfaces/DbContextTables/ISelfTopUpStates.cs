namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface ISelfTopUpStates : IDbContextTable<SelfTopUpState>
        ,IDbCanAdd<SelfTopUpState>
        ,IDbCanUpdate<SelfTopUpState>
        ,IDbCanList<SelfTopUpState>
        ,IDbCanGetById<SelfTopUpState>
        , IDbCanSearch<SelfTopUpState>
        ,IDbCanRemoveById<SelfTopUpState>
    {
        
    }
}
