using Hot.API.Client.Common;
using Hot.API.Client.Models; 
using OneOf;
using System;
using System.Threading.Tasks;

namespace Hot.API.Client.Extensions
{
    public static class ZesaTokenExtensions
    {
        public static OneOf<QueryZESAMeterResponse, APIException, string> QueryZESAMeter
           (this HotAPIClient hotAPIClient, string MeterNumber)
        {
            return QueryZESAMeterAsync(hotAPIClient, new QueryZESAMeterRequest() { MeterNumber = MeterNumber }).Result;
        }

        public static async Task<OneOf<QueryZESAMeterResponse, APIException, string>> QueryZESAMeterAsync
            (this HotAPIClient hotAPIClient, string MeterNumber)
        {
            return await QueryZESAMeterAsync(hotAPIClient, new QueryZESAMeterRequest() { MeterNumber = MeterNumber });
        }


        public static OneOf<QueryZESAMeterResponse, APIException, string> QueryZESAMeter
            (this HotAPIClient hotAPIClient, QueryZESAMeterRequest request)
        {
            return QueryZESAMeterAsync(hotAPIClient, request).Result;
        }

        public static async Task<OneOf<QueryZESAMeterResponse, APIException, string>> QueryZESAMeterAsync
            (this HotAPIClient hotAPIClient, QueryZESAMeterRequest request)
        {

            return await hotAPIClient.apiService.Post<QueryZESAMeterResponse, QueryZESAMeterRequest>(
                "agents/check-customer-zesa", request, Guid.NewGuid().ToString());
        }


        public static OneOf<ZESARechargeResponse, APIException, string> ZESATokenRecharge
            (this HotAPIClient hotAPIClient, string MeterNumber, decimal Amount, string TargetMobile, string CustomerSMS, string Reference)
        {
            return ZESATokenRechargeAsync(hotAPIClient,
                new ZESARechargeRequest()
                {
                    Amount = Amount,
                    CustomerSMS = CustomerSMS,
                    MeterNumber = MeterNumber,
                    TargetNumber = TargetMobile
                }, Reference).Result;
        }

        public static async Task<OneOf<ZESARechargeResponse, APIException, string>> ZESATokenRechargeAsync
            (this HotAPIClient hotAPIClient, string MeterNumber, decimal Amount, string TargetMobile, string CustomerSMS, string Reference)
        {
            return await ZESATokenRechargeAsync(hotAPIClient,
                new ZESARechargeRequest()
                {
                    Amount = Amount,
                    CustomerSMS = CustomerSMS,
                    MeterNumber = MeterNumber,
                    TargetNumber = TargetMobile
                }, Reference);
        }

        public static async Task<OneOf<ZESARechargeResponse, APIException, string>> ZESATokenRechargeUSDAsync
            (this HotAPIClient hotAPIClient, string MeterNumber, decimal Amount, string TargetMobile, string CustomerSMS, string Reference)
        {
            return await ZESATokenRechargeUSDAsync(hotAPIClient,
                new ZESARechargeRequest()
                {
                    Amount = Amount,
                    CustomerSMS = CustomerSMS,
                    MeterNumber = MeterNumber,
                    TargetNumber = TargetMobile
                }, Reference);
        }

        public static async Task<OneOf<ZESARechargeResponse, APIException, string>> ZESATokenRechargeAsync
            (this HotAPIClient hotAPIClient, ZESARechargeRequest request, string Reference)
        {
            return await hotAPIClient.apiService.Post<ZESARechargeResponse, ZESARechargeRequest>("agents/recharge-zesa", request, Reference);
        }

        public static async Task<OneOf<ZESARechargeResponse, APIException, string>> ZESATokenRechargeUSDAsync
            (this HotAPIClient hotAPIClient, ZESARechargeRequest request, string Reference)
        {
            return await hotAPIClient.apiService.Post<ZESARechargeResponse, ZESARechargeRequest>("agents/recharge-zesa-usd", request, Reference);
        }


        public static async Task<OneOf<ZESARechargeResponse, APIException, string>> QueryZESATokenRecharge
           (this HotAPIClient hotAPIClient, int RechargeId)
        {
            return await QueryZESATokenRechargeAsync(hotAPIClient, RechargeId);
        }

        public static async Task<OneOf<ZESARechargeResponse, APIException, string>> QueryZESATokenRechargeAsync
            (this HotAPIClient hotAPIClient, int RechargeId)
        {
            return await hotAPIClient.apiService.Get<ZESARechargeResponse, QueryZESARechargeRequest>
                ("agents/query-zesa-transaction", new QueryZESARechargeRequest { RechargeId = RechargeId });
        }

    }
}
     
