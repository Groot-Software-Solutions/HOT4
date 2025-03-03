using Hot.API.Client.Common;
using Hot.API.Client.Models;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hot.API.Client
{
    public static class NetoneEVDExtensions
    {

        public static OneOf<QueryEvdResponse, APIException, string> QueryEvdStock
            (this HotAPIClient hotAPIClient)
        {
            return QueryEvdStockAsync(hotAPIClient).Result;
        }

        public static async Task<OneOf<QueryEvdResponse, APIException, string>> QueryEvdStockAsync
            (this HotAPIClient hotAPIClient)
        {

            return await hotAPIClient.apiService.Get<QueryEvdResponse>(
                "agents/query-evd");
        }

        public static OneOf<BulkEvdResponse, APIException, string> BulkEvdSale
            (this HotAPIClient hotAPIClient, BulkEvdRequest request, string RechargeId)
        {
            return BulkEvdSaleAsync(hotAPIClient, request.BrandId, request.Denomination, request.Quantity, RechargeId).Result;
        }

        public static OneOf<BulkEvdResponse, APIException, string> BulkEvdSale
            (this HotAPIClient hotAPIClient, int BrandId, decimal Denomination, int Quantity, string RechargeId)
        {
            return BulkEvdSaleAsync(hotAPIClient, BrandId, Denomination, Quantity, RechargeId).Result;
        }

        public static async Task<OneOf<BulkEvdResponse, APIException, string>> BulkEvdSaleAsync
          (this HotAPIClient hotAPIClient, BulkEvdRequest request, string RechargeId)
        {
            return await BulkEvdSaleAsync(hotAPIClient, request.BrandId, request.Denomination, request.Quantity, RechargeId);
        }

        public static async Task<OneOf<BulkEvdResponse, APIException, string>> BulkEvdSaleAsync
            (this HotAPIClient hotAPIClient, int BrandId, decimal Denomination, int Quantity, string RechargeId)
        {
            var parameters = $"{{\"BrandID\": {BrandId},\"Denomination\":{Denomination},\"Quantity\":{Quantity}}}";

            return await hotAPIClient.apiService.Post<BulkEvdResponse, string>("agents/bulk-evd", parameters, RechargeId);
        }


        public static async Task<OneOf<BulkEvdResponse, APIException, string>> QueryEvdSaleAsync
            (this HotAPIClient hotAPIClient, string RechargeId)
        {
            return await hotAPIClient.apiService.Get<BulkEvdResponse>($"agents/query-evd-tran?RechargeId={RechargeId}");
        }


        public static OneOf<BulkEvdResponse, APIException, string> RechargeEvd
            (this HotAPIClient hotAPIClient, RechargeEvdRequest request)
        {
            return RechargeEvd(hotAPIClient, request.BrandId, request.Denomination, request.Quantity, request.TargetNumber, request.Reference);
        }

        public static OneOf<BulkEvdResponse, APIException, string> RechargeEvd
            (this HotAPIClient hotAPIClient, int BrandId, decimal Denomination, int Quantity, string TargetNumber, string Reference)
        {
            return RechargeEvdAsync(hotAPIClient, BrandId, Denomination, Quantity, TargetNumber, Reference).Result;
        }

        public static async Task<OneOf<BulkEvdResponse, APIException, string>> RechargeEvdAsync
            (this HotAPIClient hotAPIClient, RechargeEvdRequest request)
        {
            return await RechargeEvdAsync(hotAPIClient, request.BrandId, request.Denomination, request.Quantity, request.TargetNumber, request.Reference);
        }

        public static async Task<OneOf<BulkEvdResponse, APIException, string>> RechargeEvdAsync
            (this HotAPIClient hotAPIClient, int BrandId, decimal Denomination, int Quantity, string TargetNumber, string Reference)
        {
            var request = new RechargeEvdRequest(){ 
                BrandId = BrandId,  
                Denomination=Denomination, 
                Quantity=Quantity, 
                TargetNumber=TargetNumber};
            //var parameters = JsonSerializer.Serialize(request);

            return await hotAPIClient.apiService.Post<BulkEvdResponse, RechargeEvdRequest>("agents/recharge-evd", request, Reference);
        }
        public static async Task<OneOf<BulkEvdResponse, APIException, string>> RechargeEvdUSDAsync
            (this HotAPIClient hotAPIClient, RechargeEvdRequest request)
        {
            return await RechargeEvdAsync(hotAPIClient, request.BrandId, request.Denomination, request.Quantity, request.TargetNumber, request.Reference);
        }

        public static async Task<OneOf<BulkEvdResponse, APIException, string>> RechargeEvdUSDAsync
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
