using Hot.API.Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.API.Interface.Extensions
{
    public static class WalletExtensions
    {
        public static WalletBalanceResponse GetWalletBalanceUSD(this HotAPIClient hotAPIClient)
        {
            return hotAPIClient.GetWalletBalanceUSDAsync().Result;
        }

        public static async Task<WalletBalanceResponse> GetWalletBalanceUSDAsync(this HotAPIClient hotAPIClient)
        {
            return await APIHelper.ApiGetCall<WalletBalanceResponse>("agents/wallet-balance-usd");
        }

    }
}
