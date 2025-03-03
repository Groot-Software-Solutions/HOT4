using TelOne.Application.Common.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using OneOf;
using TelOne.Application.Common.Exceptions;

namespace TelOne.Infrastructure.Services
{
    public class APIService : IAPIService
    {
        public string APIName { get; set; } = "TelOne";
        public static bool DisableCertErrors { get; set; } = false;
        private readonly JsonSerializerOptions SerializerOptions;

        private IHttpClientFactory ClientFactory { get; set; }

        public APIService(IHttpClientFactory clientFactory)
        {
            ClientFactory = clientFactory;
            SerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<OneOf<T, APIException, string>> Get<T>(string url)
        {
            var ApiClient = GetApiClient();
            var response = await ApiClient.GetAsync(url);
            return await ResultObject<T>(url, response);
        }

        public async Task<OneOf<T, APIException, string>> Post<T, U>(string url, FormUrlEncodedContent parameters)
        {
            var ApiClient = GetApiClient();
            var response = await ApiClient.PostAsync(url, parameters);
            return await ResultObject<T>(url, response);
        }

        public async Task<OneOf<T, APIException, string>> Post<T, U>(string url, U parameters)
        {
            var ApiClient = GetApiClient();
            var response = await ApiClient.PostAsJsonAsync(url, parameters);
            return await ResultObject<T>(url, response);
        }

        public async Task<OneOf<T, APIException, string>> Post<T, U>(string url, U parameters, string reference)
        {
            var ApiClient = GetApiClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(parameters)
            };
            request = SetHeader(request, reference);
            using var response = await ApiClient.SendAsync(request); 
            return await ResultObject<T>(url, response);
        }


        private HttpClient GetApiClient()
        {
            return ClientFactory.CreateClient(APIName);
        }

        private HttpRequestMessage SetHeader(HttpRequestMessage httpRequest, string agentReference = "")
        {
            agentReference = (agentReference == "" ? Guid.NewGuid().ToString() : agentReference);
            httpRequest.Headers.Add("x-agent-reference", agentReference);
            return httpRequest;
        }

        private async Task<OneOf<T, APIException, string>> ResultObject<T>(string url, HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var logdata =  await response.Content.ReadAsStringAsync();
                    return await response.Content.ReadFromJsonAsync<T>(SerializerOptions);
                }
                catch (Exception)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
            else
            {
                return new APIException($"Failed Post Request - {url}", $"{ response.ReasonPhrase } - { await response.Content.ReadAsStringAsync()}");
            }
        }
         
    }
}
