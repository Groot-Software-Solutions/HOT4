using Hot.API.Client.Common;
using OneOf; 

namespace Hot.API.Client.Interfaces
{
    public interface IAPIService
    {
        string APIName { get; set; }
        bool DisableCertErrors { get; set; }

        Task<OneOf<T, APIException, string>> Get<T>(string url);
        Task<OneOf<T, APIException, string>> Get<T, U>(string url, U parameters);
        Task<OneOf<T, APIException, string>> Post<T, U>(string url, U parameters);
        Task<OneOf<T, APIException, string>> Post<T>(string url, FormUrlEncodedContent parameters);
        Task<OneOf<T, APIException, string>> Post<T, U>(string url, U parameters, string reference);
        Task<OneOf<T, APIException, string>> Post<T>(string url, FormUrlEncodedContent parameters, string reference);
        void SetAuthDetails(string accessCode, string accessPassword);
        void SetBaseURL(string Url);
        void SetSSLHandler();
         
        
    }
}
