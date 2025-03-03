namespace Hot.Application.Common.Interfaces
{
    public interface IAPIService
    {
        public string? APIName { get; set; }
        Task<OneOf<T, APIException, string>> Get<T>(string url) where T : class, new();
        Task<OneOf<T, APIException, string>> Post<T, U>(string url, U parameters) where T : class, new();
        Task<OneOf<string, APIException>> Post<U>(string url, U parameters);
        Task<OneOf<T, APIException, string>> Post<T>(string url, FormUrlEncodedContent parameters) where T : class, new();
    }
}
