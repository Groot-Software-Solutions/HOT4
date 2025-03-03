using Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.DbContext
{
    public abstract class DbContextTable<T> : IDbContextTable<T>
    {
        readonly IDbHelper dbHelper;

        protected DbContextTable(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public virtual async Task<List<T>> ListAsync()
        {
            return await dbHelper.Query<T>($"sp{typeof(T).Name}_List");
        }

        public virtual async Task<T> GetAsync(int Id)
        {
            return await dbHelper.QuerySingle<T>($"sp{typeof(T).Name}_Get @Id", new { Id });
        }

        public virtual async Task<List<T>> SearchAsync(string Id)
        {
            return await dbHelper.Query<T>($"sp{typeof(T).Name}_GetByString @Id", new {  Id });
        }

        public virtual async Task<bool> AddAsync(T Item)
        { 
            string query = $"sp{typeof(T).Name}_Add {string.Join(",",Item.GetType().GetProperties().Where(p=>p.Name.ToLower()!="id").Select(s => $"@{s.Name}").ToList())}";
            
            return await dbHelper.Execute<T>(query, Item);
        }

        public virtual async Task<bool> UpdateAsync(T Item)
        {
            string query = $"sp{typeof(T).Name}_Update {string.Join(",", Item.GetType().GetProperties().Select(s => $"@{s.Name}").ToList())}";

            return await dbHelper.Execute<T>(query, Item);
        }

        public virtual async Task<bool> RemoveAsync(int Id)
        {
            return (await dbHelper.ExecuteScalar($"sp{typeof(T).Name}_Delete @Id", Id ) == 0);
        }


    }
}
