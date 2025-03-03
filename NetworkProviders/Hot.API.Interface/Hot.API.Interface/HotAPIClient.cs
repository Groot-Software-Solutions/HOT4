using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hot.API.Interface.Models;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net.Http;
using Newtonsoft.Json;

namespace Hot.API.Interface
{
    public class HotAPIClient
    {
       
        public HotAPIClient()
        {
            APIHelper.InitializeClient();
            APIHelper.SetSSLHandler();
        }

        public HotAPIClient(string BaseURL, string AccessCode, string AccessPassword, bool DisableCertErrors = false)
        {
            APIHelper.InitializeClient();
            APIHelper.DisableCertErrors = DisableCertErrors; 
            APIHelper.SetSSLHandler();
            
            APIHelper.SetBaseURL(BaseURL);
            APIHelper.SetAuthDetails(AccessCode, AccessPassword);

        }


        #region "Agent Funtions"

        public WalletBalanceResponse GetWalletBalance()
        { 
                return GetWalletBalanceAsync().Result; 
        }

        public async Task<WalletBalanceResponse> GetWalletBalanceAsync()
        { 
                return await APIHelper.ApiGetCall<WalletBalanceResponse>("agents/wallet-balance"); 
        }

        public EndUserBalanceResponse GetEndUserBalance(string TargetMobile)
        {
            return GetEndUserBalanceAsync(TargetMobile).Result;
        }

        public async Task<EndUserBalanceResponse> GetEndUserBalanceAsync(string TargetMobile)
        { 
            using (var response = await APIHelper.ApiClient.SendAsync(
                APIHelper.SetHeader(new HttpRequestMessage(HttpMethod.Get, "agents/enduser-balance?targetMobile=" + TargetMobile)
                )))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    result = result.Replace("Expiry Date: ", "");
                    return JsonConvert.DeserializeObject<EndUserBalanceResponse>(result);
                }
                else
                {
                    throw new HotAPIException((int)response.StatusCode, HotAPIException.GetMessage(await response.Content.ReadAsStringAsync()));
                }
            }
        }

        public RechargeResponse Recharge(string TargetMobile, decimal Amount, string RechargeID, string CustomSMS = @"Hot Recharge has topped up your account with
%AMOUNT%
Initial balance $%INITIALBALANCE%
Final Balance $%FINALBALANCE%")
        {
            return RechargeAsync(TargetMobile, Amount, RechargeID, CustomSMS).Result;
        }

        public Task<RechargeResponse> RechargeAsync(string TargetMobile, decimal Amount, string RechargeID)
        {
            return RechargeAsync(TargetMobile, Amount, RechargeID, @"Hot Recharge has topped up your account with
%AMOUNT%
Initial balance $%INITIALBALANCE%
Final Balance $%FINALBALANCE%");
        }

        public async Task<RechargeResponse> RechargeAsync(string TargetMobile, decimal Amount, string RechargeID, string CustomSMS)
        { 
            var parameters = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("amount", Convert.ToString(Amount)),
                    new KeyValuePair<string, string>("targetMobile", TargetMobile),
                    new KeyValuePair<string, string>("CustomerSMS", CustomSMS )
                }); 
            return await APIHelper.APIPostCall<RechargeResponse>("agents/recharge-pinless", parameters,RechargeID);
            
        }

        public RechargeBundledResponse RechargeBundled(string TargetMobile, decimal Amount, string RechargeID, string CustomSMS = @"Hot Recharge has topped up your account with
%AMOUNT%
Initial balance $%INITIALBALANCE%
Final Balance $%FINALBALANCE%")
        {
            return RechargeUSDBundledAsync(TargetMobile, Amount, RechargeID, CustomSMS).Result;
        }

        public Task<RechargeBundledResponse> RechargeBundledAsync(string TargetMobile, decimal Amount, string RechargeID)
        {
            return RechargeUSDBundledAsync(TargetMobile, Amount, RechargeID, @"Hot Recharge has topped up your account with
%AMOUNT%
Initial balance $%INITIALBALANCE%
Final Balance $%FINALBALANCE%");
        }
        public async Task<RechargeBundledResponse> RechargeUSDBundledAsync(string TargetMobile, decimal Amount, string RechargeID, string CustomSMS)
        {
            var parameters = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("amount", Convert.ToString(Amount)),
                    new KeyValuePair<string, string>("targetMobile", TargetMobile),
                    new KeyValuePair<string, string>("CustomerSMS", CustomSMS )
                });
            return await APIHelper.APIPostCall<RechargeBundledResponse>("agents/recharge-pinless-bundled", parameters, RechargeID); 
        }

        public QueryTransactionResponse QueryTransaction(string AgentReference)
        {
            return QueryTransactionAsync(AgentReference).Result;
        }

        public async Task<QueryTransactionResponse> QueryTransactionAsync(string AgentReference)
        { 
           
            return await APIHelper.ApiGetCall<QueryTransactionResponse>(
                "agents/query-transaction?agentReference=" + AgentReference); 
        }

        public GetBundleResponse GetBundles()
        {
            return GetBundlesAsync().Result;
        }

        public async Task<GetBundleResponse> GetBundlesAsync()
        { 
            
            return await APIHelper.ApiGetCall<GetBundleResponse>(
                "agents/get-data-bundles"); 
        }

        public RechargeResponse RechargeData(string TargetMobile, string ProductCode, string RechargeID, decimal Amount = 0)
        {
            return RechargeDataAsync(TargetMobile, ProductCode, RechargeID, Amount).Result;
        }

        public async Task<RechargeResponse> RechargeDataAsync(string TargetMobile, string ProductCode, string RechargeID, decimal Amount)
        { 
            var parameters = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("productCode", ProductCode),
                    new KeyValuePair<string, string>("targetMobile", TargetMobile),
                    new KeyValuePair<string, string>("amount", Convert.ToString(Amount))
                }); 
            return await APIHelper.APIPostCall< RechargeResponse>( "agents/recharge-data", parameters,RechargeID);
            
        }

        public QueryEvdResponse QueryEvdStock()
        {
            return QueryEvdStockAsync().Result;
        }
         
        public async Task<QueryEvdResponse> QueryEvdStockAsync()
        {

            return await APIHelper.ApiGetCall<QueryEvdResponse>(
                "agents/query-evd" );
        }

        
        public BulkEvdResponse BulkEvdSale(BulkEvdRequest request, string RechargeId )
        { 
            return BulkEvdSaleAsync(request.BrandId, request.Denomination, request.Quantity, RechargeId).Result;
        }
 
        public BulkEvdResponse BulkEvdSale(int BrandId, decimal Denomination, int Quantity, string RechargeId)
        {
            return BulkEvdSaleAsync( BrandId,  Denomination,  Quantity,  RechargeId).Result;
        }

        public async Task<BulkEvdResponse> BulkEvdSaleAsync(int BrandId , decimal Denomination, int Quantity,string RechargeId)
        {
            var parameters = $"{{\"BrandID\": {BrandId},\"Denomination\":{Denomination},\"Quantity\":{Quantity}}}";

            return await APIHelper.APIPostCall<BulkEvdResponse>( "agents/bulk-evd", parameters,RechargeId);
        }


        public async Task<BulkEvdResponse> QueryEvdSaleAsync(string RechargeId)
        {
            return await APIHelper.ApiGetCall<BulkEvdResponse>($"agents/query-evd-tran?RechargeId={RechargeId}");
        }


        #endregion

        #region "Dealer Functions"

        public async Task<TransactionsGetResponse> TransactionsGetRecentAsync()
        {
            return await APIHelper.ApiGetCall<TransactionsGetResponse>("dealers/transactions-query-recent");
        }
        public TransactionsGetResponse TransactionsGetRecent()
        {
            return TransactionsGetRecentAsync().Result;
        }

        public async Task<SubscribersGetResponse> SubscribersGetListAsync()
        {
            return await APIHelper.ApiGetCall<SubscribersGetResponse>("dealers/subscribers-get-list");
        }
        public SubscribersGetResponse SubscribersGetList()
        {
            return SubscribersGetListAsync().Result;
        }

        public async Task<SubscriberDeleteResponse> SubscribersDeleteAsync(long SubscriberId)
        {
            return await APIHelper.ApiGetCall<SubscriberDeleteResponse>($"subscribers-delete?SubscriberId={SubscriberId}"); 
        }
        public SubscriberDeleteResponse SubscribersDelete(long SubscriberId)
        {
            return SubscribersDeleteAsync(SubscriberId).Result;
        }

        public async Task<SubscriberSaveResponse> SubscribersSaveAsync(string Name, string Mobile, long SubscriberId)
        {
            var parameters = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Name", Name),
                    new KeyValuePair<string, string>("Mobile", Mobile),
                    new KeyValuePair<string, string>("SubscriberId", SubscriberId.ToString() )
                });
            return await APIHelper.APIPostCall<SubscriberSaveResponse>("agents/subscribers - save", parameters);

        }
        public SubscriberSaveResponse SubscribersSave(string Name, string Mobile, long SubscriberId)
        {
            return SubscribersSaveAsync(Name, Mobile, SubscriberId).GetAwaiter().GetResult();
        }


        #endregion

    }


}
