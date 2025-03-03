using Hot.API.Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hot.API.Interface.Extensions
{
    public static class DataBundleExtensions
    {

        public static GetBundleResponse GetBundles
            (this HotAPIClient hotAPIClient)
        {
            return GetBundlesAsync(hotAPIClient).Result;
        }

        public static async Task<GetBundleResponse> GetBundlesAsync
            (this HotAPIClient hotAPIClient)
        {

            return await APIHelper.ApiGetCall<GetBundleResponse>(
                "agents/get-data-bundles");
        }
        public static async Task<GetBundleResponse> GetBundlesUSDAsync
           (this HotAPIClient hotAPIClient)
        {

            return await APIHelper.ApiGetCall<GetBundleResponse>(
                "agents/get-data-bundles-usd");
        }

        public static RechargeResponse RechargeData
            (this HotAPIClient hotAPIClient, string TargetMobile, string ProductCode, string RechargeID, decimal Amount = 0)
        {
            return RechargeDataAsync(hotAPIClient, TargetMobile, ProductCode, RechargeID, Amount).Result;
        }

        public static async Task<RechargeResponse> RechargeDataAsync
            (this HotAPIClient hotAPIClient, string TargetMobile, string ProductCode, string RechargeID, decimal Amount = 0)
        {
            var parameters = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("productCode", ProductCode),
                    new KeyValuePair<string, string>("targetMobile", TargetMobile),
                    new KeyValuePair<string, string>("amount", Convert.ToString(Amount))
                });
            return await APIHelper.APIPostCall<RechargeResponse>("agents/recharge-data", parameters, RechargeID);

        }
        public static async Task<RechargeResponse> RechargeDataUSDAsync
            (this HotAPIClient hotAPIClient, string TargetMobile, string ProductCode, string RechargeID, decimal Amount = 0)
        {
            var parameters = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("productCode", ProductCode),
                    new KeyValuePair<string, string>("targetMobile", TargetMobile),
                    new KeyValuePair<string, string>("amount", Convert.ToString(Amount))
                });
            return await APIHelper.APIPostCall<RechargeResponse>("agents/recharge-data-usd", parameters, RechargeID);

        }

        public static async Task<RechargeResponse> RechargeDataAsync
            (this HotAPIClient hotAPIClient, string TargetMobile, string ProductCode, string RechargeID, string CustomSMS, decimal Amount = 0)
        {
            var parameters = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("productCode", ProductCode),
                    new KeyValuePair<string, string>("targetMobile", TargetMobile),
                    new KeyValuePair<string, string>("amount", Convert.ToString(Amount)),
                    new KeyValuePair<string, string>("CustomerSMS", CustomSMS )
                });
            return await APIHelper.APIPostCall<RechargeResponse>("agents/recharge-data", parameters, RechargeID);

        }


    }
}
