using Hot.API.Client.Interfaces;

namespace Hot.API.Client
{
    public class HotAPIClient
    {
        internal readonly IAPIService apiService;

        public HotAPIClient(IAPIService apiService)
        {
            this.apiService = apiService;
        }

        public void SetOptions(string BaseURL, string AccessCode, string AccessPassword, bool DisableCertErrors = false)
        {
            apiService.DisableCertErrors = DisableCertErrors;
            apiService.SetSSLHandler(); 
            apiService.SetBaseURL(BaseURL);
            apiService.SetAuthDetails(AccessCode, AccessPassword);

        }

       
    }


}
