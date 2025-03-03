
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Hot.API.Interface
{
    public static class APIHelper
    {
        public static HttpClient ApiClient { get; set; }
        public static bool DisableCertErrors { get; set; } = false;

        public static void InitializeClient()
        {
            ApiClient = new HttpClient();
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        }

        public static void SetBaseURL(string Url)
        {
            ApiClient.BaseAddress = new Uri(Url);
        }

        public static void SetAuthDetails(string accessCode, string accessPassword)
        {
            ApiClient.DefaultRequestHeaders.Add("x-access-code", accessCode);
            ApiClient.DefaultRequestHeaders.Add("x-access-password", accessPassword);
        }

        public static HttpRequestMessage SetHeader(HttpRequestMessage httpRequest, string agentReference = "")
        {
            agentReference = (agentReference == "" ? Guid.NewGuid().ToString() : agentReference);
            httpRequest.Headers.Add("x-agent-reference", agentReference);
            return httpRequest;
        }

        public static async Task<T> ApiGetCall<T>(string url)
        {
            using (var response = await ApiClient.SendAsync(
                            SetHeader(new HttpRequestMessage(HttpMethod.Get, url)
                            )))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<T>();
                }
                else
                {
                    throw new HotAPIException((int)response.StatusCode, HotAPIException.GetMessage(await response.Content.ReadAsStringAsync()));
                }
            }
        }

        public static async Task<T> APIPostCall<T>(string url, FormUrlEncodedContent parameters, string agentReference = "")
        {
            using (var response = await ApiClient.SendAsync(
                SetHeader(
                    new HttpRequestMessage(HttpMethod.Post, url)
                    {
                        Content = parameters
                    }, agentReference)))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<T>();
                }
                else
                {
                    throw new HotAPIException((int)response.StatusCode, HotAPIException.GetMessage(await response.Content.ReadAsStringAsync()));
                }
            }
        }
      
        public static async Task<T> APIPostCall<T>(string url, string parameters, string agentReference = "")
        {
            using (var response = await ApiClient.SendAsync(
                SetHeader(
                    new HttpRequestMessage(HttpMethod.Post, url)
                    {
                        Content = new StringContent(parameters, Encoding.UTF8, "application/json")
                }, agentReference)))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<T>();
                }
                else
                {
                    throw new HotAPIException((int)response.StatusCode, HotAPIException.GetMessage(await response.Content.ReadAsStringAsync()));
                }
            }
        }


        public static void SetSSLHandler()
        {
            ServicePointManager.ServerCertificateValidationCallback += delegate (object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                if (sslPolicyErrors == SslPolicyErrors.None)
                {
                    return true;   //Is valid
                }

                if (DisableCertErrors)
                {
                    return true;
                }

                return false;
            };

        }

    }
}
