using Hot.API.Client.Common;
using Hot.API.Client.Interfaces;
using Hot.API.Client.Models;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.API.Client
{
    public static class AccountExtensions
    {
        public static OneOf<WalletBalanceResponse, APIException, string> GetWalletBalance(this HotAPIClient hotAPIClient)
        {
            return GetWalletBalanceAsync(hotAPIClient).Result;
        }

        public static async Task<OneOf<WalletBalanceResponse, APIException,string>> GetWalletBalanceAsync(this HotAPIClient hotAPIClient)
{
            var result =  
                await hotAPIClient.apiService.Get<WalletBalanceResponse>("agents/wallet-balance");
            return result;
        }

        public static async Task<OneOf<WalletBalanceResponse, APIException, string>> GetWalletBalanceUSDAsync(this HotAPIClient hotAPIClient)
        {
            var result =
                await hotAPIClient.apiService.Get<WalletBalanceResponse>("agents/wallet-balance-usd");
            return result;
        }

        public static async Task<OneOf<Response, APIException, string>> ResetPinAsync
            (this HotAPIClient hotAPIClient, string targetNumber, string names, string iDNumber, string sender)
        {
            return await hotAPIClient.apiService.Post<Response, PinResetRequest>
              ("dealers/resetpin", new PinResetRequest
              {
                  TargetNumber = targetNumber,
                  Names = names,
                  RequestingNumber = sender,
                  IDNumber = iDNumber
              });
        }

        public static OneOf<Response, APIException, string> ResetPin
            (this HotAPIClient hotAPIClient, string targetNumber, string names, string iDNumber, string sender)
        {
            return ResetPinAsync(hotAPIClient, targetNumber, names, iDNumber, sender).Result;
        }

    }
}
