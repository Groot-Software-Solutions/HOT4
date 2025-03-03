using Dapper;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Exceptions;
using Microsoft.Extensions.Configuration;
using OneOf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Diagnostics;
using System.Data.Common;

namespace Hot.Infrastructure.DbContext
{
    public class DbHelper : IDbHelper
    {
        private readonly IConfiguration configuration;

        public DbHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string ConnectionString(string? dbconnection = "DefaultConnection")
        {
            dbconnection = string.IsNullOrEmpty(dbconnection) ? "DefaultConnection" : dbconnection;
            return configuration.GetConnectionString(dbconnection)??"";
        }

        public IDbConnection Connection(string? dbConnection = null)
        {
            var CnnVal = ConnectionString(dbConnection);
            
           IDbConnection connection = new SqlConnection(CnnVal);
            return connection;  
        }

        private static HotDbException DbException<T>(string query, T parameters, Exception ex, string Name)
        {
            return new HotDbException( 
                ex.Message,
                new Exception($"{Name} Method",
                new Exception($"Exception: {ex.Message}; Query: { query }; Parameters: {JsonSerializer.Serialize(parameters)}")
                ));
        } 

        public async Task<OneOf<List<T>, HotDbException>> Query<T>(string query, object? parameters = null, string? dbconnection = null)
        {
            try
            {
                var CnnVal = ConnectionString(dbconnection);
                using IDbConnection connection = new SqlConnection(CnnVal);
                return (await connection.QueryAsync<T>(query, parameters)).ToList();
            }
            catch (Exception ex)
            {
                return DbException(query, parameters, ex, "Query");
            }

        }

        public async Task<OneOf<T, HotDbException>> QuerySingle<T>(string query, object parameters, string? dbconnection = null)
        {
            try
            {
                var CnnVal = ConnectionString(dbconnection);
                using IDbConnection connection = new SqlConnection(CnnVal);
                return (await connection.QueryFirstAsync<T>(query, parameters));
            }
            catch (Exception ex)
            {
                return DbException(query, parameters, ex, "QuerySingle");
            }
        }

        public async Task<OneOf<bool, HotDbException>> Execute<T>(string query, T parameters, string? dbconnection = null)
        {
            try
            {
                var CnnVal = ConnectionString(dbconnection);
                using IDbConnection connection = new SqlConnection(CnnVal);
                await connection.ExecuteAsync(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                return DbException(query, parameters, ex, "Execute");
            }
        }

        public async Task<OneOf<U, HotDbException>> ExecuteScalar<U, T>(string query, T parameters, string? dbconnection = null)
        {
            try
            {
                var CnnVal = ConnectionString(dbconnection);
                using IDbConnection connection = new SqlConnection(CnnVal);
                return (await connection.ExecuteScalarAsync<U>(query, parameters));
            }
            catch (Exception ex)
            {
                return DbException(query, parameters, ex, "ExecuteScalar<U,T>");
            }
        }

        public Task<OneOf<int, HotDbException>> ExecuteScalar<T>(string query, T parameters, string? dbconnection = null)
        {
            return ExecuteScalar<int, T>(query, parameters, dbconnection);
        }

        public async Task<OneOf<bool, HotDbException>> Execute<T>(string query, T parameters, IDbConnection connection, IDbTransaction transaction)
        {
            try
            {
                await connection.ExecuteAsync(query, parameters, transaction);
                return true;
            }
            catch (Exception ex)
            {
                return DbException(query, parameters, ex, "ExecuteScalar<T>");
            }
        }

        public async Task<OneOf<U, HotDbException>> ExecuteScalar<U, T>(string query, T parameters, IDbConnection connection, IDbTransaction transaction)
        {
            try
            {
                return (await connection.ExecuteScalarAsync<U>(query, parameters,transaction));
            }
            catch (Exception ex)
            {
                return DbException(query, parameters, ex, "ExecuteScalar<U,T>");
            }
        }
         
        public Task<OneOf<int, HotDbException>> ExecuteScalar<T>(string query, T parameters, IDbConnection connection, IDbTransaction transaction)
        {
            return ExecuteScalar<int, T>(query, parameters, connection,transaction);
        }

        public async Task<OneOf<Tuple<IDbConnection, IDbTransaction>, HotDbException>> BeginTransaction(string dbconnection = "")
        {
            var CnnVal = ConnectionString(dbconnection);
            var connection = new SqlConnection(CnnVal);
            connection.Open();
            var transaction = await connection.BeginTransactionAsync();
            return new Tuple<IDbConnection, IDbTransaction>(connection, transaction);
        }

        public OneOf<bool, HotDbException> CommitTransaction(IDbTransaction transaction)
        {
            try
            {
                transaction.Commit(); 
                //transaction.Connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                return DbException("Commit Transaction", "", ex, "CommitTransaction");
            } 

        }

        public OneOf<bool, HotDbException> RollBackTransaction(IDbTransaction transaction)
        {
            try
            {
                transaction.Rollback();
                //transaction.Connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                return DbException("Rollback Transaction", "", ex, "Rollback");
            }

        }


    }
}
