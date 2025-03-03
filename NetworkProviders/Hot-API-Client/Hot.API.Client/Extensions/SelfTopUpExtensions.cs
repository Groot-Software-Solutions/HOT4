using Hot.API.Client.Common;
using Hot.API.Client.Models;
using OneOf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hot.API.Client.Extensions
{
    public static class SelfTopUpExtensions
    {

        public static async Task<OneOf<Response,APIException,string>> SelfTopSaveAsync
            (this HotAPIClient hotAPIClient,decimal Amount, string TargetMobile, string BillerMobile, string Currency)
        {

            return await hotAPIClient.apiService.Post<Response, SelfTopUpRequest>
                ("dealers/selftopup", new SelfTopUpRequest
                {
                    Amount = Amount,
                    TargetMobile = TargetMobile,
                    BillerMobile = BillerMobile,
                    Currency = Currency
                },
                Guid.NewGuid().ToString());

        }
       
        public static OneOf<Response, APIException, string> SelfTopSave
            (this HotAPIClient hotAPIClient, decimal Amount, string TargetMobile, string BillerMobile, string Currency)
        {
            return SelfTopSaveAsync(hotAPIClient,Amount, TargetMobile, BillerMobile,Currency).GetAwaiter().GetResult();
        }

        
    }
}
