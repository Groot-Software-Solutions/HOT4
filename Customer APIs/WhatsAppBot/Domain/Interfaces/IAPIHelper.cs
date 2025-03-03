using System.Net.Http;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IAPIHelper
    {
        public string APIName { get; set; }
        Task<T> ApiGetCall<T>(string url);
        Task<T> APIPostCall<T>(string url, FormUrlEncodedContent parameters);
        Task<T> APIPostCall<T,U>(string url, U parameters); 
        Task<T> APIPostCall<T, U>(string url, U parameters, string reference);
    }
}