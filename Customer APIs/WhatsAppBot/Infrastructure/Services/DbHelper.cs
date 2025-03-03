using Dapper;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{

    public class DbHelper : IDbHelper
    {
        private readonly ConfigHelper helper;

        public DbHelper(ConfigHelper helper)
        {
            this.helper = helper;
        }

        public async Task<List<T>> Query<T>(string query, object parameters = null, string dbconnection = "")
        {
            var CnnVal = string.IsNullOrEmpty(dbconnection) ? helper.CnnVal() : helper.CnnVal(dbconnection);
            using IDbConnection connection = new SqlConnection(CnnVal);
            return (await connection.QueryAsync<T>(query, parameters)).ToList();
        }

        public async Task<T> QuerySingle<T>(string query, object parameters, string dbconnection = "")
        {
            var CnnVal = string.IsNullOrEmpty(dbconnection) ? helper.CnnVal() : helper.CnnVal(dbconnection);
            using IDbConnection connection = new SqlConnection(CnnVal);
            return (await connection.QueryFirstAsync<T>(query, parameters));
        }

        public async Task<bool> Execute<T>(string query, T parameters, string dbconnection = "")
        {
            try
            {
                var CnnVal = string.IsNullOrEmpty(dbconnection) ? helper.CnnVal() : helper.CnnVal(dbconnection);
                using IDbConnection connection = new SqlConnection(CnnVal);
                _ = (await connection.ExecuteAsync(query, parameters));
                return true; 
            }
            catch
            {
                return false;
            } 
        }

        public async Task<T> ExecuteScalar<T>(string query, T parameters, string dbconnection = "")
        {
            var CnnVal = string.IsNullOrEmpty(dbconnection) ? helper.CnnVal() : helper.CnnVal(dbconnection);
            using IDbConnection connection = new SqlConnection(CnnVal);
            return (await connection.ExecuteScalarAsync<T>(query, parameters));

        }

        public async Task<U> ExecuteScalar<T, U>(string query, T parameters, string dbconnection = "")
        {
            var CnnVal = string.IsNullOrEmpty(dbconnection) ? helper.CnnVal() : helper.CnnVal(dbconnection);
            using IDbConnection connection = new SqlConnection(CnnVal);
            return (await connection.ExecuteScalarAsync<U>(query, parameters));
        }
    }
}
