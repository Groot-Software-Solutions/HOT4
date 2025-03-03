using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sage.Application.Common.Interfaces
{
    public interface IDbHelper
    {
        public Task<bool> Execute<T>(string query, T parameters, string dbconnection = "");
        public Task<T> ExecuteScalar<T>(string query, T parameters, string dbconnection = "");
        public Task<U> ExecuteScalar<T, U>(string query, T parameters, string dbconnection = "");
        public Task<T> QuerySingle<T,U>(string query, U parameters, string dbconnection = "");
        public Task<List<T>> Query<T,U>(string query, U parameters, string dbconnection = "");
        public Task<List<T>> Query<T>(string query, string dbconnection = "");
    }
        
}
