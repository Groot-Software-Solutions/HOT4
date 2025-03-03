using Sage.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sage.Infrastructure.Services
{
    public class APIService : IAPIService
    {

        private IHttpClientFactory ClientFactory { get; set; }

        public string APIName { get; set; }
        private readonly JsonSerializerOptions SerializerOptions;
        public static bool DisableCertErrors { get; set; } = false;

        public APIService(IHttpClientFactory clientFactory)
        {
            ClientFactory = clientFactory;
            SerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        }

        public async Task<T> APIGetCall<T>(string url)
        {
            var ApiClient = APIName != null 
                ? ClientFactory.CreateClient(APIName)
                : ClientFactory.CreateClient();

            using var response = await ApiClient
                .SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<T>(SerializerOptions);
                }
                catch (Exception ex)
                {
                    throw new Exception($"API Serialization Error - {ex.Message}", new Exception(await response.Content.ReadAsStringAsync()));
                }
            }
            else
            {
                throw new Exception(response.ReasonPhrase, new Exception(await response.Content.ReadAsStringAsync()));
            }
        }

        public async Task<T> APIPostCall<T>(string url, FormUrlEncodedContent parameters)
        {
            var ApiClient = APIName != null
               ? ClientFactory.CreateClient(APIName)
               : ClientFactory.CreateClient();
            using var response = await ApiClient.SendAsync(
                    new HttpRequestMessage(HttpMethod.Post, url)
                    {
                        Content = parameters
                    });
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<T>(SerializerOptions);
                }
                catch (Exception ex)
                {
                    throw new Exception($"API Serialization Error - {ex.Message}", new Exception(await response.Content.ReadAsStringAsync()));
                }
            }
            else
            {
                throw new Exception(response.ReasonPhrase, new Exception(await response.Content.ReadAsStringAsync()));
            }
        }

        public async Task<T> APIPostCall<T, U>(string url, U parameters)
        {
            var ApiClient = APIName != null
                ? ClientFactory.CreateClient(APIName)
                : ClientFactory.CreateClient();

            JsonContent jsonContent = JsonContent.Create(parameters);
            var test = jsonContent.ReadAsStringAsync().Result;
            using var response = await ApiClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = jsonContent
            });

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var test2 = response.Content.ReadAsStringAsync().Result.ToString();
                    return await response.Content.ReadFromJsonAsync<T>(SerializerOptions);
                }
                catch (Exception ex)
                {
                    throw new Exception($"API Serialization Error - {ex.Message}", new Exception(await response.Content.ReadAsStringAsync()));
                }
            }
            else
            {
                throw new Exception(response.ReasonPhrase, new Exception(await response.Content.ReadAsStringAsync()));
            }
        }

        public async Task<T> APIDeleteCall<T>(string url)
        {
            var ApiClient = APIName != null
                 ? ClientFactory.CreateClient(APIName)
                 : ClientFactory.CreateClient();

            using var response = await ApiClient
                .SendAsync(new HttpRequestMessage(HttpMethod.Delete, url));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<T>(SerializerOptions);
                }
                catch (Exception ex)
                {
                    throw new Exception($"API Serialization Error - {ex.Message}", new Exception(await response.Content.ReadAsStringAsync()));
                }
            }
            else
            {
                throw new Exception(response.ReasonPhrase, new Exception(await response.Content.ReadAsStringAsync()));
            }
        }

    }
}
