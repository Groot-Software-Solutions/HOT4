using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class APIHelper : IAPIHelper
    {
        public APIHelper(IHttpClientFactory clientFactory)
        {
            ClientFactory = clientFactory;
            SerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        private IHttpClientFactory ClientFactory { get; set; }

        public string APIName { get; set; } = "WhatsApp";
        private readonly JsonSerializerOptions SerializerOptions;
        public async Task<T> ApiGetCall<T>(string url)
        {
            try
            {

                var ApiClient = ClientFactory.CreateClient(APIName);

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
            catch (Exception ex)
            {
                throw new Exception($"API Remote Error - {ex.Message}", new Exception(ex.Message));
            }
        }

        public async Task<T> APIPostCall<T>(string url, FormUrlEncodedContent parameters)
        {
            try
            {

                var ApiClient = ClientFactory.CreateClient(APIName);
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
            catch (Exception ex)
            {
                throw new Exception($"API Remote Error - {ex.Message}", new Exception(ex.Message));
            }
        }

        public async Task<T> APIPostCall<T, U>(string url, U parameters)
        {
            try
            {
                var ApiClient = ClientFactory.CreateClient(APIName);
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
            catch (Exception ex)
            {
                throw new Exception($"API Remote Error - {ex.Message}", new Exception(ex.Message));
            }

        }

        public async Task<T> APIPostCall<T, U>(string url, U parameters, string reference)
        {
            try
            {

                var ApiClient = ClientFactory.CreateClient(APIName);
                JsonContent jsonContent = JsonContent.Create(parameters);
                var test = jsonContent.ReadAsStringAsync().Result;
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = jsonContent
                };
                using var response = await ApiClient.SendAsync(SetHeader(request, reference));

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
            catch (Exception ex)
            {
                throw new Exception($"API Remote Error - {ex.Message}", new Exception(ex.Message));
            }
        }

        private HttpRequestMessage SetHeader(HttpRequestMessage httpRequest, string agentReference = "")
        {
            agentReference = (agentReference == "" ? Guid.NewGuid().ToString() : agentReference);
            httpRequest.Headers.Add("x-agent-reference", agentReference);
            return httpRequest;
        }
    }

}
