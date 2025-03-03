namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface IWebRequests : IDbContextTable<WebRequest>
        , IDbCanAdd<WebRequest>
        , IDbCanUpdate<WebRequest>
    {
        public Task<OneOf<WebRequest, HotDbException>> GetAsync(string AgentReference, int AccessID);
        public OneOf<WebRequest, HotDbException> Get(string AgentReference, int AccessID);

    }
}
