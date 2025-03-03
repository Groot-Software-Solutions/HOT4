using Hot.API.Client.Common;
using Hot.API.Client.Models;
using OneOf;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hot.API.Client
{
    public static class EconetDataBundleExtensions
    {
         
        public static OneOf<GetBundleResponse, APIException,string> GetBundles
            (this HotAPIClient hotAPIClient)
        {
            return GetBundlesAsync(hotAPIClient).Result;
        }

        public static async Task<OneOf<GetBundleResponse, APIException,string>> GetBundlesAsync
            (this HotAPIClient hotAPIClient)
        {

            return await hotAPIClient.apiService.Get<GetBundleResponse>(
                "agents/get-data-bundles");
        }
        public static async Task<OneOf<GetBundleResponse, APIException, string>> GetBundlesUSDAsync
           (this HotAPIClient hotAPIClient)
        {

            return await hotAPIClient.apiService.Get<GetBundleResponse>(
                "agents/get-data-bundles-usd");
        }

        public static OneOf<RechargeResponse, APIException,string> RechargeData
            (this HotAPIClient hotAPIClient, string TargetMobile, string ProductCode, string RechargeID, decimal Amount = 0)
        {
            return RechargeDataAsync(hotAPIClient,TargetMobile, ProductCode, RechargeID, Amount).Result;
        }

        public static async Task<OneOf<RechargeResponse, APIException,string>> RechargeDataAsync
            (this HotAPIClient hotAPIClient, string TargetMobile, string ProductCode, string RechargeID, decimal Amount = 0)
        {
            var parameters = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("productCode", ProductCode),
                    new KeyValuePair<string, string>("targetMobile", TargetMobile),
                    new KeyValuePair<string, string>("amount", Convert.ToString(Amount))
                });
            return await hotAPIClient.apiService.Post<RechargeResponse>("agents/recharge-data", parameters, RechargeID);

        }
        public static async Task<OneOf<RechargeResponse, APIException, string>> RechargeDataUSDAsync
            (this HotAPIClient hotAPIClient, string TargetMobile, string ProductCode, string RechargeID, decimal Amount = 0)
        {
            var parameters = (Amount  == 0 )
                ? new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("ProductCode", ProductCode),
                    new KeyValuePair<string, string>("TargetMobile", TargetMobile), 
                })
                :  new FormUrlEncodedContent(new[]
               {
                    new KeyValuePair<string, string>("productCode", ProductCode),
                    new KeyValuePair<string, string>("targetMobile", TargetMobile),
                    new KeyValuePair<string, string>("amount", Convert.ToString(Amount))
                });
            return await hotAPIClient.apiService.Post<RechargeResponse>("agents/recharge-data-usd", parameters, RechargeID);

        }

        public static async Task<OneOf<RechargeResponse, APIException,string>> RechargeDataAsync
            (this HotAPIClient hotAPIClient, string TargetMobile, string ProductCode, string RechargeID, string CustomSMS, decimal Amount = 0)
        {
            var parameters = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("productCode", ProductCode),
                    new KeyValuePair<string, string>("targetMobile", TargetMobile),
                    new KeyValuePair<string, string>("amount", Convert.ToString(Amount)),
                    new KeyValuePair<string, string>("CustomerSMS", CustomSMS )
                });
            return await hotAPIClient.apiService.Post<RechargeResponse>("agents/recharge-data", parameters, RechargeID);

        }

        public static async Task<OneOf<RechargeResponse, APIException, string>> RechargeDataUSDAsync
            (this HotAPIClient hotAPIClient, string TargetMobile, string ProductCode, string RechargeID, string CustomSMS, decimal Amount = 0)
        {
            var parameters = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("productCode", ProductCode),
                    new KeyValuePair<string, string>("targetMobile", TargetMobile),
                    new KeyValuePair<string, string>("amount", Convert.ToString(Amount)),
                    new KeyValuePair<string, string>("CustomerSMS", CustomSMS )
                });
            return await hotAPIClient.apiService.Post<RechargeResponse>("agents/recharge-data-usd", parameters, RechargeID);

        }


    }
}
