using Hot.Application.Common.Interfaces; 

namespace Hot.Tests.Common.Fakers.DbContext;

public class DbFakerService
{
    public readonly IDbContext dbContext;

    public DbFakerService(IDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

}
public static class DBFakerServiceExtesion
{
    
    public static DbFakerService Gives(this IDbContext dbContext)
    {
        return new DbFakerService(dbContext);
    }
}
