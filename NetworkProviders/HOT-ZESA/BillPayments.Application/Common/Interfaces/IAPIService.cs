using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BillPayments.Application.Common.Interfaces
{
    public interface IAPIService
    {
        public string APIName { get; set; }
        Task<T> ApiGetCall<T>(string url);
        Task<T> APIPostCall<T>(string url, FormUrlEncodedContent parameters);
        Task<T> APIPostCall<T, U>(string url, U parameters);
        Task<T> APIPostCall<T, U>(string url, U parameters, string reference);
    }
}
