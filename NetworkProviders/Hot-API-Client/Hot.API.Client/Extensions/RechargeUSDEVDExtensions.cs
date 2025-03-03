using Hot.API.Client.Common;
using Hot.API.Client.Models;
using OneOf; 

namespace Hot.API.Client.Extensions
{
    public static class RechargeUSDEVDExtensions
    {

        public static OneOf<BulkEvdResponse, APIException, string> RechargeUSDEvd
           (this HotAPIClient hotAPIClient, RechargeEvdRequest request)
        {
            return RechargeUSDEvd(hotAPIClient, request.BrandId, request.Denomination, request.Quantity, request.TargetNumber, request.Reference);
        }

        public static OneOf<BulkEvdResponse, APIException, string> RechargeUSDEvd
            (this HotAPIClient hotAPIClient, int BrandId, decimal Denomination, int Quantity, string TargetNumber, string Reference)
        {
            return RechargeUSDEvdAsync(hotAPIClient, BrandId, Denomination, Quantity, TargetNumber, Reference).Result;
        }

        public static async Task<OneOf<BulkEvdResponse, APIException, string>> RechargeUSDEvdAsync
            (this HotAPIClient hotAPIClient, RechargeEvdRequest request)
        {
            return await RechargeUSDEvdAsync(hotAPIClient, request.BrandId, request.Denomination, request.Quantity, request.TargetNumber, request.Reference);
        }

        public static async Task<OneOf<BulkEvdResponse, APIException, string>> RechargeUSDEvdAsync
            (this HotAPIClient hotAPIClient, int BrandId, decimal Denomination, int Quantity, string TargetNumber, string Reference)
        {
            var request = new RechargeEvdRequest()
            {
                BrandId = BrandId,
                Denomination = Denomination,
                Quantity = Quantity,
                TargetNumber = TargetNumber
            };
            //var parameters = JsonSerializer.Serialize(request);

            return await hotAPIClient.apiService.Post<BulkEvdResponse, RechargeEvdRequest>("agents/recharge-evd-usd", request, Reference);
        }
    }
}
