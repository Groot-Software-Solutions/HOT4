using Dapper;
using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Interfaces;
using Hot.Domain.Entities;
using OneOf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Infrastructure.DbContext
{
    public abstract class Table<T> : IDbContextTable<T>,
          IDbCanAdd<T>, IDbCanAddInTransaction<T>, IDbCanGetById<T>, IDbCanList<T>, IDbCanRemoveById<T>,
         IDbCanSearch<T>, IDbCanUpdate<T>, IDbCanUpdateInTransaction<T>
    {

        public virtual string StoredProcedurePrefix { get; set; } = "sp";
        public virtual string? IdPrefix { get; set; } = null;
        public virtual string ListSuffix { get; set; } = "_List";
        public virtual string GetSuffix { get; set; } = "_Get";
        public virtual string SearchSuffix { get; set; } = "_GetByString";
        public virtual string? SearchParameter { get; set; } = null;
        public virtual string AddSuffix { get; set; } = "_Add";
        public virtual string? AddParameters { get; set; } = null;
        public virtual string UpdateSuffix { get; set; } = "_Update";
        public virtual string? UpdateParameters { get; set; } = null;
        public virtual string DeleteSuffix { get; set; } = "_Delete";
        public readonly IDbHelper dbHelper;

        protected Table(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public virtual async Task<OneOf<List<T>, HotDbException>> ListAsync()
        {
            string query = $"{GetSPPrefix()}{ListSuffix}";
            var result = await dbHelper.Query<T>(query);
            return result;
        }

        public virtual async Task<OneOf<T, HotDbException>> GetAsync(int Id)
        {
            string query = $"{GetSPPrefix()}{GetSuffix} @{IdPrefix ?? typeof(T).Name}Id";
            var parameters = new Dictionary<string, object>() { { $"@{IdPrefix ?? typeof(T).Name}Id", Id } };
            var result = await dbHelper.QuerySingle<T>(query, parameters);
            return result;
        }
        public virtual async Task<OneOf<T, HotDbException>> GetAsync(long Id)
        {
            string query = $"{GetSPPrefix()}{GetSuffix} @{IdPrefix ?? typeof(T).Name}Id";
            var parameters = new Dictionary<string, object>() { { $"@{IdPrefix ?? typeof(T).Name}Id", Id } };
            var result = await dbHelper.QuerySingle<T>(query, parameters);
            return result;
        }

        public virtual async Task<OneOf<List<T>, HotDbException>> SearchAsync(string Id)
        {
            string query = $"{GetSPPrefix()}{SearchSuffix} @{SearchParameter ?? typeof(T).Name}";
            var parameters = new Dictionary<string, object>() { { $"@{SearchParameter ?? typeof(T).Name}", Id } };
            var result = await dbHelper.Query<T>(query, parameters);
            return result;
        }
         

        public virtual async Task<OneOf<int, HotDbException>> AddAsync(T Item)
        {
            string query = $"{GetSPPrefix()}{AddSuffix} { AddParameters ?? GetParameterListWithoutId(Item)}";
            var result  = await dbHelper.ExecuteScalar<int, T>(query, Item);
            return result;
        }

        public virtual async Task<OneOf<int, HotDbException>> AddAsync(T Item, IDbConnection connection, IDbTransaction transaction)
        {
            string query = $"{GetSPPrefix()}{AddSuffix} { AddParameters ?? GetParameterListWithoutId(Item)}";
            var result = await dbHelper.ExecuteScalar<int, T>(query, Item, connection, transaction);
            return result;
        }

        public virtual async Task<OneOf<bool, HotDbException>> UpdateAsync(T Item)
        {
            string query = $"{GetSPPrefix()}{UpdateSuffix} {UpdateParameters ?? GetParameterListAll(Item)}";
            var result = await dbHelper.Execute(query, Item);
            return result;
        }

        public virtual async Task<OneOf<bool, HotDbException>> UpdateAsync(T Item, IDbConnection connection, IDbTransaction transaction)
        {
            string query = $"{GetSPPrefix()}{UpdateSuffix} {UpdateParameters ?? GetParameterListAll(Item)}";
            var result = await dbHelper.Execute(query, Item, connection, transaction);
            return result;
        }

        public virtual async Task<OneOf<bool, HotDbException>> RemoveAsync(int Id)
        {
            string query = $"{GetSPPrefix()}{DeleteSuffix} @{IdPrefix ?? typeof(T).Name}Id";
            var parameters = new Dictionary<string, object>() { { $"@{IdPrefix ?? typeof(T).Name}Id", Id } };
            var result = await dbHelper.Execute(query, parameters);
            return result;
        }

        public OneOf<int, HotDbException> Add(T Item)
        {
            return AddAsync(Item).Result;
        }
        public OneOf<int, HotDbException> Add(T Item, System.Data.IDbConnection connection, System.Data.IDbTransaction transaction)
        {
            return AddAsync(Item, connection, transaction).Result;
        }
        public OneOf<T, HotDbException> Get(int Id)
        {
            return GetAsync(Id).Result;
        }
        public OneOf<T, HotDbException> Get(long Id)
        {
            return GetAsync(Id).Result;
        }
        public OneOf<List<T>, HotDbException> Search(string Id)
        {
            return SearchAsync(Id).Result;
        }
        public OneOf<List<T>, HotDbException> List()
        {
            return ListAsync().Result;
        }
        public OneOf<bool, HotDbException> Remove(int Id)
        {
            return RemoveAsync(Id).Result;
        }
        public OneOf<bool, HotDbException> Update(T Item)
        {
            return UpdateAsync(Item).Result;
        }

        public OneOf<bool, HotDbException> Update(T Item, IDbConnection connection, IDbTransaction transaction)
        {
            return UpdateAsync(Item, connection, transaction).Result;
        }

        private static string GetParameterListAll(T Item)
        {
            if (Item is null) return "";
            List<string> parameters = Item.GetType().GetProperties()
                .Select(s => $"@{s.Name}")
                .ToList();
            return string.Join(",", parameters);
        }
        private string GetParameterListWithoutId(T Item)
        {
            if (Item is null) return "";
            var objectProperties =  Item.GetType().GetProperties()
                .Where(p => p.Name.ToLower() != $"{IdPrefix}id")
                .Select(s => $"@{s.Name}")
                .ToList(); 
            return string.Join(",", objectProperties);
        }

        internal string GetSPPrefix()
        {
            return $"{StoredProcedurePrefix}{typeof(T).Name}";
        }

    }

}
