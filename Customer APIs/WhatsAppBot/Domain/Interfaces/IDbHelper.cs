using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IDbHelper
    {
        public Task<bool> Execute<T>(string query, T parameters, string dbconnection = "");
        public Task<T> ExecuteScalar<T>(string query, T parameters, string dbconnection = ""); 
        public Task<U> ExecuteScalar<T,U>(string query, T parameters, string dbconnection = "");
        public Task<T> QuerySingle<T>(string query, object parameters, string dbconnection = "");
        public Task<List<T>> Query<T>(string query, object parameters = null, string dbconnection = "");
    }
}
