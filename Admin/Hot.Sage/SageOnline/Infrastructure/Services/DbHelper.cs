using Dapper;
using Microsoft.Extensions.Configuration;
using Sage.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sage.Infrastructure.Services
{
    public class DbHelper: IDbHelper
    {
        private readonly IConfiguration configuration;

        public DbHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<List<T>> Query<T,U>(string query, U parameters , string dbconnection = "")
        {
            var CnnVal = string.IsNullOrEmpty(dbconnection) ? configuration.GetConnectionString("DefaultConnection") : configuration.GetConnectionString(dbconnection);
            using IDbConnection connection = new SqlConnection(CnnVal);
            return (await connection.QueryAsync<T>(query, parameters)).ToList();
        }

        public async Task<T> QuerySingle<T,U>(string query, U parameters, string dbconnection = "")
        {
            var CnnVal = string.IsNullOrEmpty(dbconnection) ? configuration.GetConnectionString("DefaultConnection") : configuration.GetConnectionString(dbconnection);
            using IDbConnection connection = new SqlConnection(CnnVal);
            return (await connection.QueryFirstAsync<T>(query, parameters));
        }

        public async Task<bool> Execute<T>(string query, T parameters, string dbconnection = "")
        {
            try
            {
                var CnnVal = string.IsNullOrEmpty(dbconnection) ? configuration.GetConnectionString("DefaultConnection") : configuration.GetConnectionString(dbconnection);
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
            var CnnVal = string.IsNullOrEmpty(dbconnection) ? configuration.GetConnectionString("DefaultConnection") : configuration.GetConnectionString(dbconnection);
            using IDbConnection connection = new SqlConnection(CnnVal);
            return (await connection.ExecuteScalarAsync<T>(query, parameters));

        }

        public async Task<U> ExecuteScalar<T, U>(string query, T parameters, string dbconnection = "")
        {
            var CnnVal = string.IsNullOrEmpty(dbconnection) ? configuration.GetConnectionString("DefaultConnection") : configuration.GetConnectionString(dbconnection);
            using IDbConnection connection = new SqlConnection(CnnVal);
            return (await connection.ExecuteScalarAsync<U>(query, parameters));
        }

        public async Task<List<T>> Query<T>(string query, string dbconnection = "")
        {
            var CnnVal = string.IsNullOrEmpty(dbconnection) ? configuration.GetConnectionString("DefaultConnection") : configuration.GetConnectionString(dbconnection);
            using IDbConnection connection = new SqlConnection(CnnVal);
            return (await connection.QueryAsync<T>(query)).ToList();
        }

    }
}
