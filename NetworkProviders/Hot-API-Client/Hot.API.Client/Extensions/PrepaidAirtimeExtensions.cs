using Hot.API.Client.Models;
using OneOf;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Hot.API.Client.Common;

namespace Hot.API.Client
{
    public static class PrepaidAirtimeExtensions
    {
        public static OneOf<EndUserBalanceResponse, APIException, string> GetEndUserBalance
            (this HotAPIClient hotClientAPI, string TargetMobile)
        {
            return GetEndUserBalanceAsync(hotClientAPI, TargetMobile).Result;
        }

        public static async Task<OneOf<EndUserBalanceResponse, APIException, string>> GetEndUserBalanceAsync
            (this HotAPIClient hotClientAPI, string TargetMobile)
        {
            var response = await hotClientAPI.apiService.Get<EndUserBalanceResponse>($"agents/enduser-balance?targetMobile={TargetMobile}");
            return response.Match
                (
                data => data,
                error =>
                {
                    throw error;
                },
                content =>
                {
                    throw new APIException("GetEndUserBalanceAsync", content);
                });
        }

        public static OneOf<RechargeResponse, APIException, string> Recharge
            (this HotAPIClient hotClientAPI, string TargetMobile, decimal Amount, string RechargeID, string CustomSMS = @"Hot Recharge has topped up your account with
%AMOUNT%
Initial balance $%INITIALBALANCE%
Final Balance $%FINALBALANCE%")
        {
            return RechargeAsync(hotClientAPI, TargetMobile, Amount, RechargeID, CustomSMS).Result;
        }

        public static Task<OneOf<RechargeResponse, APIException, string>> RechargeAsync
            (this HotAPIClient hotClientAPI, string TargetMobile, decimal Amount, string RechargeID)
        {
            return RechargeAsync(hotClientAPI, TargetMobile, Amount, RechargeID, @"Hot Recharge has topped up your account with
%AMOUNT%
Initial balance $%INITIALBALANCE%
Final Balance $%FINALBALANCE%");
        }

        public static async Task<OneOf<RechargeResponse, APIException, string>> RechargeAsync
            (this HotAPIClient hotClientAPI, string TargetMobile, decimal Amount, string RechargeID, string CustomSMS)
        {
            //var parameters = new FormUrlEncodedContent(new[]
            //    {
            //        new KeyValuePair<string, string>("amount", Convert.ToString(Amount)),
            //        new KeyValuePair<string, string>("targetMobile", TargetMobile),
            //        new KeyValuePair<string, string>("CustomerSMS", CustomSMS )
            //    });
            var request = new RechargeRequest() 
            { 
                Amount = Amount,  
                TargetMobile = TargetMobile ,
                CustomerSMS = CustomSMS,
            };
            var result = await hotClientAPI.apiService.Post<RechargeResponse,RechargeRequest>("agents/recharge-pinless", request, RechargeID);
            return result;
        }

        public static Task<OneOf<RechargeResponse, APIException, string>> RechargeUSDAsync
            (this HotAPIClient hotClientAPI, string TargetMobile, decimal Amount, string RechargeID)
        {
            return RechargeUSDAsync(hotClientAPI, TargetMobile, Amount, RechargeID, @"Hot Recharge has topped up your account with
%AMOUNT%
Initial balance $%INITIALBALANCE%
Final Balance $%FINALBALANCE%");
        }

        public static async Task<OneOf<RechargeResponse, APIException, string>> RechargeUSDAsync
            (this HotAPIClient hotClientAPI, string TargetMobile, decimal Amount, string RechargeID, string CustomSMS)
        {
            var request = new RechargeRequest()
            {
                Amount = Amount,
                TargetMobile = TargetMobile,
                CustomerSMS = CustomSMS,
            };
            var result = await hotClientAPI.apiService.Post<RechargeResponse, RechargeRequest>("agents/recharge-pinless-usd", request, RechargeID); 
            return result;
        } 
       
        public static OneOf<RechargeResponse, APIException, string> RechargeUSD
            (this HotAPIClient hotClientAPI, string TargetMobile, decimal Amount, string RechargeID, string CustomSMS = @"Hot Recharge has topped up your account with
%AMOUNT%
Initial balance $%INITIALBALANCE%
Final Balance $%FINALBALANCE%")
        {
            return RechargeUSDAsync(hotClientAPI, TargetMobile, Amount, RechargeID, CustomSMS).Result;
        } 
    }
}
