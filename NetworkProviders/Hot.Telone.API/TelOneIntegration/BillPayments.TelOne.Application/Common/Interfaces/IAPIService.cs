using OneOf;
using System.Net.Http;
using System.Threading.Tasks;
using TelOne.Application.Common.Exceptions;

namespace TelOne.Application.Common.Interfaces
{
    public interface IAPIService
    {
        Task<OneOf<T, APIException, string>> Get<T>(string url);
        Task<OneOf<T, APIException, string>> Post<T, U>(string url, U parameters);
        Task<OneOf<T, APIException, string>> Post<T, U>(string url, FormUrlEncodedContent parameters);
        Task<OneOf<T, APIException, string>> Post<T, U>(string url, U parameters, string reference);

        string APIName { get; set; }
         
    }
}