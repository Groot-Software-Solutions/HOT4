using System.Data;

namespace Hot.Application.Common.Interfaces
{
    public interface IDbHelper
    {
        public Task<OneOf<List<T>, HotDbException>> Query<T>(string query, object? parameters = null, string dbconnection = "");
        public Task<OneOf<T, HotDbException>> QuerySingle<T>(string query, object parameters, string dbconnection = "");
        public Task<OneOf<bool, HotDbException>> Execute<T>(string query, T parameters, string dbconnection = "");
        public Task<OneOf<U, HotDbException>> ExecuteScalar<U,T>(string query, T parameters, string dbconnection = "");
        public Task<OneOf<int, HotDbException>> ExecuteScalar<T>(string query, T parameters, string dbconnection = "");
        public Task<OneOf<U, HotDbException>> ExecuteScalar<U, T>(string query, T parameters, IDbConnection connection, IDbTransaction transaction);
        public Task<OneOf<bool, HotDbException>> Execute<T>(string query, T parameters, IDbConnection connection, IDbTransaction transaction);
        public Task<OneOf<Tuple<IDbConnection, IDbTransaction>, HotDbException>> BeginTransaction(string dbconnection = "");
        public OneOf<bool, HotDbException> CommitTransaction(IDbTransaction transaction);
        public Task<OneOf<int, HotDbException>> ExecuteScalar<T>(string query, T parameters, IDbConnection connection, IDbTransaction transaction);
        public OneOf<bool, HotDbException> RollBackTransaction(IDbTransaction transaction); 
        public string ConnectionString(string? dbconnection = "DefaultConnection");
        public IDbConnection Connection(string? dbConnection = null);
    }
}
