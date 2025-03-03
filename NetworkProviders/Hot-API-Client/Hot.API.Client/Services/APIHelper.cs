
using Hot.API.Client.Common;
using Hot.API.Client.Interfaces;
using Newtonsoft.Json;
using OneOf;
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

namespace Hot.API.Client
{
    public class APIHelper : IAPIService
    {

        public string APIName { get; set; } = "";
        public bool DisableCertErrors { get; set; } = false;
        private IHttpClientFactory ClientFactory { get; set; }
        private string AccessCode { get; set; } = "";
        private string AccessPassword { get; set; } = "";
        private Uri BaseAddress { get; set; } 

        public APIHelper(IHttpClientFactory clientFactory)
        {
            ClientFactory = clientFactory; 
        }

        public async Task<OneOf<T, APIException, string>> Get<T>(string url)
        {
            var ApiClient = GetApiClient(); 
            ApiClient.DefaultRequestHeaders.Add("x-agent-reference", Guid.NewGuid().ToString());
            var response = await ApiClient.GetAsync(url);
            return await ResultObject<T>(url, response);
        }

        public async Task<OneOf<T, APIException, string>> Get<T,U>(string url, U request)
        {
            var ApiClient = GetApiClient();
            ApiClient.DefaultRequestHeaders.Add("x-agent-reference", Guid.NewGuid().ToString());
            List<string> parameters = request.GetType().GetProperties()
                .Select(s => $"{s.Name}={s.GetValue(request)}")
                .ToList();
            var query = string.Join("&", parameters);
            var response = await ApiClient.GetAsync($"{url}?{query}");
            return await ResultObject<T>(url, response);
        }


        public async Task<OneOf<T, APIException, string>> Post<T>(string url, FormUrlEncodedContent parameters)
        {
            var ApiClient = GetApiClient();
            var response = await ApiClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, url) { Content = parameters });
            return await ResultObject<T>(url, response);
        }

        public async Task<OneOf<T, APIException, string>> Post<T>(string url, FormUrlEncodedContent parameters, string reference)
        {
            var ApiClient = GetApiClient();
            var response = await ApiClient.SendAsync(SetHeader(new HttpRequestMessage(HttpMethod.Post, url) { Content = parameters }, reference));
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
            HttpRequestMessage request = new(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(parameters)
            };
            request = SetHeader(request, reference);
            var response = await ApiClient.SendAsync(request); 
            return await ResultObject<T>(url, response);

        }


        private HttpClient GetApiClient()
        {
            var client =  ClientFactory.CreateClient(APIName);
            client.BaseAddress = BaseAddress;
            client.DefaultRequestHeaders.Add("x-access-code", AccessCode);
            client.DefaultRequestHeaders.Add("x-access-password", AccessPassword);
            return client;
        }
         
        private static HttpRequestMessage SetHeader(HttpRequestMessage httpRequest, string? agentReference = null)
        { 
            httpRequest.Headers.Add("x-agent-reference", agentReference ?? Guid.NewGuid().ToString()); 
            return httpRequest;
        }

        private async Task<OneOf<T, APIException, string>> ResultObject<T>(string url, HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    //var result=  await response.Content.ReadFromJsonAsync<T>(SerializerOptions);
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<T>(content);
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return await response.Content.ReadAsStringAsync();
                }
            }
            else
            {
                try
                {
                    //var result=  await response.Content.ReadFromJsonAsync<T>(SerializerOptions);
                    var content = await response.Content.ReadAsStringAsync(); 
                    return content;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new APIException($"Failed Post Request - {url}", $"{ response.ReasonPhrase } - { await response.Content.ReadAsStringAsync()}");
                }
               
            }
        }

        public void SetAuthDetails(string accessCode, string accessPassword)
        {
            AccessCode = accessCode;
            AccessPassword = accessPassword;
        }

        public void SetBaseURL(string Url)
        {
            BaseAddress = new Uri(Url);
        }

        public void SetSSLHandler()
        {
            ServicePointManager.ServerCertificateValidationCallback += delegate (object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                if (sslPolicyErrors == SslPolicyErrors.None) return true;   //Is valid 
                if (DisableCertErrors) return true;
                return false;
            };
        }

    }
}
