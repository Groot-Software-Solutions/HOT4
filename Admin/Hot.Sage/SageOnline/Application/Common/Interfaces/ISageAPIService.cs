using Sage.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sage.Application.Common.Interfaces 
{
    public interface ISageAPIService
    {
        Task<SageResponse<T>> Get<T>(string OrderBy = "", int Skip = 0, int Limit = 0);
        Task<SageResponse<T>> Get<T>(int Id);
        Task<SageResponse<T>> Get<T>(long Id);
        Task<SageResponse<T>> GetAll<T>();
        Task<SageResponse<T>> Save<T>(T item);
        Task<SageResponse<U>> Save<T,U>(T item);
        Task<SageResponse<T>> SaveBatch<T>(List<T> list);
        Task<SageResponse<T>> Delete<T>(int Id);
        Task<SageResponse<T>> SaveSingle<T>(T item);
    }

    
}
