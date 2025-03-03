using Hot.API.Client.Common;
using Hot.API.Client.Models;
using OneOf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hot.API.Client.Extensions
{
    public static class TeloneDataExtensions
    {

        public static OneOf<QueryTeloneAccountResponse, APIException, string> QueryTeloneAccount
           (this HotAPIClient hotAPIClient, string AccountNumber)
        {
            return QueryTeloneAccountAsync(hotAPIClient, new QueryTeloneAccountRequest() { AccountNumber = AccountNumber }).Result;
        }

        public static async Task<OneOf<QueryTeloneAccountResponse, APIException, string>> QueryTeloneAccountAsync
            (this HotAPIClient hotAPIClient, string AccountNumber)
        {
            return await QueryTeloneAccountAsync(hotAPIClient, new QueryTeloneAccountRequest() { AccountNumber = AccountNumber });
        }


        public static OneOf<QueryTeloneAccountResponse, APIException, string> QueryTeloneAccount
            (this HotAPIClient hotAPIClient, QueryTeloneAccountRequest request)
        {
            return QueryTeloneAccountAsync(hotAPIClient, request).Result;
        }

        public static async Task<OneOf<QueryTeloneAccountResponse, APIException, string>> QueryTeloneAccountAsync
            (this HotAPIClient hotAPIClient, QueryTeloneAccountRequest request)
        {

            return await hotAPIClient.apiService.Get<QueryTeloneAccountResponse, QueryTeloneAccountRequest>(
                "agents/verify-telone-account", request);
        }




        public static OneOf<QueryTeloneBundlesResponse, APIException, string> QueryTeloneBundles
            (this HotAPIClient hotAPIClient)
        {
            return QueryTeloneBundlesAsync(hotAPIClient).Result;
        }

        public static async Task<OneOf<QueryTeloneBundlesResponse, APIException, string>> QueryTeloneBundlesAsync
            (this HotAPIClient hotAPIClient)
        {
            return await hotAPIClient.apiService.Get<QueryTeloneBundlesResponse>
                ("agents/query-telone-bundles");
        }

        public static OneOf<QueryTeloneBundlesResponse, APIException, string> QueryTeloneBundlesUSD
           (this HotAPIClient hotAPIClient)
        {
            return QueryTeloneBundlesUSDAsync(hotAPIClient).Result;
        }

        public static async Task<OneOf<QueryTeloneBundlesResponse, APIException, string>> QueryTeloneBundlesUSDAsync
            (this HotAPIClient hotAPIClient)
        {
            return await hotAPIClient.apiService.Get<QueryTeloneBundlesResponse>
                ("agents/query-telone-bundles-usd");
        }


        public static OneOf<TeloneBundlePurchaseResponse, APIException, string> BulkTeloneEvd
            (this HotAPIClient hotAPIClient, TeloneBulkEvdRequest request)
        {
            return BulkTeloneEvdAsync(hotAPIClient, request).Result;
        }

        public static OneOf<TeloneBundlePurchaseResponse, APIException, string> BulkTeloneEvd
            (this HotAPIClient hotAPIClient, int ProductId, int Quantity)
        {
            return BulkTeloneEvdAsync(hotAPIClient, ProductId, Quantity).Result;
        }

        public static async Task<OneOf<TeloneBundlePurchaseResponse, APIException, string>> BulkTeloneEvdAsync
            (this HotAPIClient hotAPIClient, TeloneBulkEvdRequest request)
        {
            return await hotAPIClient.apiService.Post<TeloneBundlePurchaseResponse, TeloneBulkEvdRequest>
                ("agents/bulk-telone-evd", request);
        }

        public static async Task<OneOf<TeloneBundlePurchaseResponse, APIException, string>> BulkTeloneEvdAsync
            (this HotAPIClient hotAPIClient, int ProductId, int Quantity)
        {
            return await BulkTeloneEvdAsync(hotAPIClient,
                new TeloneBulkEvdRequest() { ProductId = ProductId, Quantity = Quantity }
                );
        }



        public static OneOf<TeloneBundlePurchaseResponse, APIException, string> RechargeTeloneEvd
           (this HotAPIClient hotAPIClient, TeloneBundlePurchaseRequest request, string Reference)
        {
            return RechargeTeloneEvdAsync(hotAPIClient, request, Reference).Result;
        }

        public static OneOf<TeloneBundlePurchaseResponse, APIException, string> RechargeTeloneEvd
           (this HotAPIClient hotAPIClient, int ProductId, string AccountNumber, string TargetNumber, string Reference)
        {
            return RechargeTeloneEvdAsync(hotAPIClient, ProductId, AccountNumber, TargetNumber, Reference).Result;
        }

        public static async Task<OneOf<TeloneBundlePurchaseResponse, APIException, string>> RechargeTeloneEvdAsync
            (this HotAPIClient hotAPIClient, TeloneBundlePurchaseRequest request, string Reference)
        {
            return await hotAPIClient.apiService.Post<TeloneBundlePurchaseResponse, TeloneBundlePurchaseRequest>
                ("agents/recharge-telone-adsl", request, Reference);
        }

        public static async Task<OneOf<TeloneBundlePurchaseResponse, APIException, string>> RechargeTeloneEvdAsync
            (this HotAPIClient hotAPIClient, int ProductId, string AccountNumber, string TargetNumber, string Reference)
        {
            return await RechargeTeloneEvdAsync(hotAPIClient, new TeloneBundlePurchaseRequest()
            { ProductId = ProductId, AccountNumber = AccountNumber, TargetNumber = TargetNumber }
            , Reference
            );
        }

        public static async Task<OneOf<TeloneBundlePurchaseResponse, APIException, string>> RechargeTeloneEvdUSDAsync
            (this HotAPIClient hotAPIClient, TeloneBundlePurchaseRequest request, string Reference)
        {
            return await hotAPIClient.apiService.Post<TeloneBundlePurchaseResponse, TeloneBundlePurchaseRequest>
                ("agents/recharge-telone-adsl-usd", request, Reference);
        }

        public static async Task<OneOf<TeloneBundlePurchaseResponse, APIException, string>> RechargeTeloneEvdUSDAsync
            (this HotAPIClient hotAPIClient, int ProductId, string AccountNumber, string TargetNumber, string Reference)
        {
            return await RechargeTeloneEvdUSDAsync(hotAPIClient, new TeloneBundlePurchaseRequest()
            { ProductId = ProductId, AccountNumber = AccountNumber, TargetNumber = TargetNumber }
            , Reference
            );
        }


    }


}
namespace Hot.API.Client.Models
{
    public class QueryTeloneAccountRequest
    {
        public string AccountNumber { get; set; } = String.Empty;
    }

    public class QueryTeloneAccountResponse : Response
    {
        public string AccountNumber { get; set; } = String.Empty;
        public string AccountName { get; set; } = String.Empty;
        public string AccountAddress { get; set; } = String.Empty;
    }


}
