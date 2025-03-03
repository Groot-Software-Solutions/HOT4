
using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Interfaces;
using OneOf;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hot.Infrastructure.Services
{

    public class APIService : IAPIService
    {
        public string? APIName { get; set; }
        private readonly JsonSerializerOptions SerializerOptions;
        public IHttpClientFactory ClientFactory { get; set; }

        public APIService(IHttpClientFactory clientFactory)
        {
            ClientFactory = clientFactory;
            SerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<OneOf<T, APIException, string>> Get<T>(string url) where T : class, new()
        {
            var ApiClient = GetHttpClient();
            using var response = await ApiClient
                .SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            return await ResultObject<T>(url, response);
        }

        public async Task<OneOf<T, APIException, string>> Post<T, U>(string url, U parameters) where T : class, new()
        {
            var ApiClient = GetHttpClient();
            using var response = await ApiClient
                .PostAsJsonAsync(url, parameters);
            return await ResultObject<T>(url, response);
        }

        public async Task<OneOf<string, APIException>> Post<U>(string url, U parameters)
        {
            var ApiClient = GetHttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonSerializer.Serialize(parameters), Encoding.UTF8, "application/json")
            };
            using var response = await ApiClient
                .SendAsync(request);
            return await ResultObject(url, response);
        }

        public async Task<OneOf<T, APIException, string>> Post<T>(string url, FormUrlEncodedContent parameters) where T : class, new()
        {
            var ApiClient = GetHttpClient();
            using var response = await ApiClient.SendAsync(
                    new HttpRequestMessage(HttpMethod.Post, url)
                    {
                        Content = parameters
                    });
            return await ResultObject<T>(url, response);
        }

        private HttpClient GetHttpClient()
        {
            return APIName != null
               ? ClientFactory.CreateClient(APIName)
               : ClientFactory.CreateClient();
        }

        private async Task<OneOf<T, APIException, string>> ResultObject<T>(string url, HttpResponseMessage response) where T : class, new()
        {
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var data = await response.Content.ReadAsStringAsync();
                    if (data.Contains("messageId")) return data;
                    return await response.Content.ReadFromJsonAsync<T>(SerializerOptions) ?? new();
                }
                catch (Exception)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
            else
            {
                return new APIException($"Failed Post Request - {url}", $"{response.ReasonPhrase} - {await response.Content.ReadAsStringAsync()}");
            }
        }
        private async Task<OneOf<string, APIException>> ResultObject(string url, HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return new APIException($"Failed Post Request - {url}", $"{response.ReasonPhrase} - {await response.Content.ReadAsStringAsync()}");
            }
        }

    }


}
