using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IDbContextTable<T>
    {
        Task<bool> AddAsync(T Item);
        Task<T> GetAsync(int Id);
        Task<List<T>> SearchAsync(string Id);
        Task<List<T>> ListAsync();
        Task<bool> RemoveAsync(int Id);
        Task<bool> UpdateAsync(T Item);
    }
}