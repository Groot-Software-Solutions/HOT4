using Hot.Lib.Entities;

namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface ILogs : IDbContextTable<Log>
        , IDbCanAdd<Log>
    {
    }
}
