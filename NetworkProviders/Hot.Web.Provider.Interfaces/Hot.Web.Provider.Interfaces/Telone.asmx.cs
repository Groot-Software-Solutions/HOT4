using Hot.API.Interface;
using Hot.Web.Provider.Interfaces.Models.TelOne;
using Hot.Web.Provider.Interfaces.Services.Telone;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Services;

namespace Hot.Web.Provider.Interfaces
{
    /// <summary>
    /// Summary description for Telone
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Telone : WebService
    {
        public static string EconetAppKey = ConfigurationManager.AppSettings["EconetAppKey"];
        private readonly string baseUrl = ConfigurationManager.AppSettings["TeloneAPIBaseUrl"];
        private readonly bool TeloneAPIDisabled = Convert.ToBoolean(ConfigurationManager.AppSettings["TeloneDisabled"]);

        [WebMethod]
        public GetBundleResponse GetAvailableBundles(string AppKey)
        {
            if (AppKey != EconetAppKey) return
                   new GetBundleResponse()
                   {
                       ReplyMessage = "Login Failed with AppKey",
                       ReplyCode = (int)ReplyCode.FailedWebLogin
                   };

            if (TeloneAPIDisabled) return
                    new GetBundleResponse()
                    {
                        ReplyMessage = "Provider Error Code: 36188",
                        ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform
                    }; ;

            var result = new GetBundleResponse();
            var client = new TelOneClient(baseUrl);

            try
            {

                var response = client.QueryBroadbandProductsAsync().Result;
                result.ReplyCode = (int)ReplyCode.Success;
                result.List = response.Select(r => (BroadbandProductItem)r).ToList();

            }
            catch (Exception ex)
            {
                result.ReplyCode = (int)ReplyCode.FailedWebException;
                result.ReplyMessage = ex.Message;
            }
            return result;
        }

        [WebMethod]
        public GetBundleResponse GetAvailableBundlesUSD(string AppKey)
        {
            if (AppKey != EconetAppKey) return
                   new GetBundleResponse()
                   {
                       ReplyMessage = "Login Failed with AppKey",
                       ReplyCode = (int)ReplyCode.FailedWebLogin
                   };

            if (TeloneAPIDisabled) return
                    new GetBundleResponse()
                    {
                        ReplyMessage = "Provider Error Code: 36188",
                        ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform
                    }; ;

            var result = new GetBundleResponse();
            var client = new TelOneClient(baseUrl);

            try
            {

                var response = client.QueryBroadbandProductsUSDAsync().Result;
                result.ReplyCode = (int)ReplyCode.Success;
                result.List = response.Select(r => BroadbandProductItem.MapUSD(r)).ToList();

            }
            catch (Exception ex)
            {
                result.ReplyCode = (int)ReplyCode.FailedWebException;
                result.ReplyMessage = ex.Message;
            }
            return result;
        }

        [WebMethod]
        public AccountBalanceResponse GetBalance(string AppKey)
        {
            if (AppKey != EconetAppKey) return
                   new AccountBalanceResponse()
                   {
                       ReplyMessage = "Login Failed with AppKey",
                       ReplyCode = (int)ReplyCode.FailedWebLogin
                   };

            if (TeloneAPIDisabled) return
                    new AccountBalanceResponse()
                    {
                        ReplyMessage = "Provider Error Code: 36188",
                        ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform
                    }; ;

            var result = new AccountBalanceResponse();
            var client = new TelOneClient(baseUrl);

            try
            {

                var response = client.QueryMerchantBalanceAsync().Result;
                result.ReplyCode = (int)ReplyCode.Success;
                result.Result = response;
                result.Balance = (decimal)response.Where(b => b.Currency.ToUpper().Replace(" ","").Contains("ZWG")).FirstOrDefault().Balance;
            }
            catch (Exception ex)
            {
                result.ReplyCode = (int)ReplyCode.FailedWebException;
                result.ReplyMessage = ex.Message;
            }
            return result;
        }
        [WebMethod]
        public AccountBalanceResponse GetBalanceUSD(string AppKey)
        {
            if (AppKey != EconetAppKey) return
                   new AccountBalanceResponse()
                   {
                       ReplyMessage = "Login Failed with AppKey",
                       ReplyCode = (int)ReplyCode.FailedWebLogin
                   };

            if (TeloneAPIDisabled) return
                    new AccountBalanceResponse()
                    {
                        ReplyMessage = "Provider Error Code: 36188",
                        ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform
                    }; ;

            var result = new AccountBalanceResponse();
            var client = new TelOneClient(baseUrl);

            try
            {

                var response = client.QueryMerchantBalanceAsync().Result;
                result.ReplyCode = (int)ReplyCode.Success;
                result.Result = response;
                result.Balance = (decimal)response.Where(b => b.Currency.ToUpper().Contains("USD")).FirstOrDefault().Balance;
            }
            catch (Exception ex)
            {
                result.ReplyCode = (int)ReplyCode.FailedWebException;
                result.ReplyMessage = ex.Message;
            }
            return result;
        }

        [WebMethod]
        public VerifyAccountResponse VerifiyUserAccount(string AppKey, string AccountId)
        {
            if (AppKey != EconetAppKey) return
                   new VerifyAccountResponse()
                   {
                       ReplyMessage = "Login Failed with AppKey",
                       ReplyCode = (int)ReplyCode.FailedWebLogin
                   };

            if (TeloneAPIDisabled) return
                    new VerifyAccountResponse()
                    {
                        ReplyMessage = "Provider Error Code: 36188",
                        ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform
                    }; ;

            var result = new VerifyAccountResponse();
            var client = new TelOneClient(baseUrl);

            try
            {
                var response = client.VerifyCustomerAccountAsync(AccountId).Result;
                result.Result = response;
                result.ReplyMessage = response.ResponseDescription;
                result.ReplyCode = response.IsSuccessful() ? (int)ReplyCode.Success : (int)ReplyCode.FailedRechargeInvalidEndUserOrNetwork;

            }
            catch (Exception ex)
            {
                result.ReplyCode = (int)ReplyCode.FailedWebException;
                result.ReplyMessage = ex.Message;
            }
            return result;
        }

        [WebMethod]
        public CustomerVoiceBalanceResponse EndUserVoiceBalance(string AppKey, string AccountId)
        {
            if (AppKey != EconetAppKey) return
                   new CustomerVoiceBalanceResponse()
                   {
                       ReplyMessage = "Login Failed with AppKey",
                       ReplyCode = (int)ReplyCode.FailedWebLogin
                   };

            if (TeloneAPIDisabled) return
                    new CustomerVoiceBalanceResponse()
                    {
                        ReplyMessage = "Provider Error Code: 36188",
                        ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform
                    }; ;

            var result = new CustomerVoiceBalanceResponse();
            var client = new TelOneClient(baseUrl);

            try
            {
                var response = client.CustomerVoiceBalanceAsync(AccountId).Result;
                result.Result = response;
                result.ReplyMessage = response.ResultHeaderField.ResultDescField;
                result.ReplyCode = response.IsSuccessful() ? (int)ReplyCode.Success : (int)ReplyCode.FailedRechargeInvalidEndUserOrNetwork;
                result.Balance = response.IsSuccessful()
                    ? response.GetTotalAmountField() == 0 ? 0 : response.GetTotalAmountField() / 10000
                    : 0;
            }
            catch (Exception ex)
            {
                result.ReplyCode = (int)ReplyCode.FailedWebException;
                result.ReplyMessage = ex.Message;
            }
            return result;
        }

        [WebMethod]
        public BulkPurchaseBroadbandResponse BulkAdslPurchaseBroadband(string AppKey, int ProductId, int Quantity, int RechargeId)
        {
            if (AppKey != EconetAppKey) return
                   new BulkPurchaseBroadbandResponse()
                   {
                       ReplyMessage = "Login Failed with AppKey",
                       ReplyCode = (int)ReplyCode.FailedWebLogin
                   };

            if (TeloneAPIDisabled) return
                    new BulkPurchaseBroadbandResponse()
                    {
                        ReplyMessage = "Provider Error Code: 36188",
                        ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform
                    }; ;

            var result = new BulkPurchaseBroadbandResponse();
            var client = new TelOneClient(baseUrl);

            try
            {
                var response = client.PurchaseBroadProductsAsync("Hot-" + RechargeId.ToString(), ProductId, Quantity).Result;
                result.Result = response;
                result.ReplyMessage = response.ResponseDescription;
                result.ReplyCode = response.IsSuccessful() ? (int)ReplyCode.Success : (int)ReplyCode.FailedNetworkPrepaidPlatform;

                if (response.ResponseDescription.ToLower().Contains("task was canceled")) throw new Exception(response.ResponseDescription);

                if (response.ResponseDescription.ToLower().Contains("sufficient")) result.ReplyMessage = "Provider Error Code: 36180";

            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("task was canceled")) throw new Exception(ex.Message);
                if (ex.Message.ToLower().Contains("sufficient")) result.ReplyMessage = "Provider Error Code: 36180";
                result.ReplyCode = (int)ReplyCode.FailedWebException;
                result.ReplyMessage = ex.Message;
            }
            return result;
        }


        [WebMethod]
        public RechargeAccountResponse RechargeAccountAdsl(string AppKey, string AccountId, int productId, int RechargeId)
        {
            if (AppKey != EconetAppKey) return
                   new RechargeAccountResponse()
                   {
                       ReplyMessage = "Login Failed with AppKey",
                       ReplyCode = (int)ReplyCode.FailedWebLogin
                   };

            if (TeloneAPIDisabled) return
                    new RechargeAccountResponse()
                    {
                        ReplyMessage = "Provider Error Code: 36188",
                        ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform
                    }; ;

            var result = new RechargeAccountResponse();
            var client = new TelOneClient(baseUrl);

            try
            {
                var response = client.RechargeAdslAccountAsync("Hot-" + RechargeId.ToString(), AccountId, productId).Result;
                result.Result = response;
                result.ReplyMessage = response.ResponseDescription;
                result.ReplyCode = response.IsSuccessful() ? (int)ReplyCode.Success : (int)ReplyCode.FailedNetworkPrepaidPlatform;

                if (response.ResponseDescription.ToLower().Contains("task was canceled")) throw new Exception(response.ResponseDescription);

                if (response.ResponseDescription.ToLower().Contains("sufficient")) result.ReplyMessage = "Provider Error Code: 36180";

            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("task was canceled")) throw new Exception(ex.Message);
                if (ex.Message.ToLower().Contains("sufficient")) result.ReplyMessage = "Provider Error Code: 36180";
                result.ReplyCode = (int)ReplyCode.FailedWebException;
                result.ReplyMessage = ex.Message;
            }
            return result;
        }

        [WebMethod]
        public RechargeAccountResponse RechargeAccountAdslUSD(string AppKey, string AccountId, int productId, int RechargeId)
        {
            if (AppKey != EconetAppKey) return
                   new RechargeAccountResponse()
                   {
                       ReplyMessage = "Login Failed with AppKey",
                       ReplyCode = (int)ReplyCode.FailedWebLogin
                   };

            if (TeloneAPIDisabled) return
                    new RechargeAccountResponse()
                    {
                        ReplyMessage = "Provider Error Code: 36188",
                        ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform
                    }; ;

            var result = new RechargeAccountResponse();
            var client = new TelOneClient(baseUrl);

            try
            {
                var response = client.RechargeAdslAccountUSDAsync("Hot-" + RechargeId.ToString(), AccountId, productId).Result;
                result.Result = response;
                result.ReplyMessage = response.ResponseDescription;
                result.ReplyCode = response.IsSuccessful() ? (int)ReplyCode.Success : (int)ReplyCode.FailedNetworkPrepaidPlatform;

                if (response.ResponseDescription.ToLower().Contains("task was canceled")) throw new Exception(response.ResponseDescription);

                if (response.ResponseDescription.ToLower().Contains("sufficient")) result.ReplyMessage = "Provider Error Code: 36180";

            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("task was canceled")) throw new Exception(ex.Message);
                if (ex.Message.ToLower().Contains("sufficient")) result.ReplyMessage = "Provider Error Code: 36180";
                result.ReplyCode = (int)ReplyCode.FailedWebException;
                result.ReplyMessage = ex.Message;
            }
            return result;
        }

        [WebMethod]
        public PayAccountBillResponse PayBill(string AppKey, string AccountId, decimal Amount, int RechargeId)
        {
            if (AppKey != EconetAppKey) return
                   new PayAccountBillResponse()
                   {
                       ReplyMessage = "Login Failed with AppKey",
                       ReplyCode = (int)ReplyCode.FailedWebLogin
                   };

            if (TeloneAPIDisabled) return
                    new PayAccountBillResponse()
                    {
                        ReplyMessage = "Provider Error Code: 36188",
                        ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform
                    }; ;

            var result = new PayAccountBillResponse();
            var client = new TelOneClient(baseUrl);

            try
            {
                var response = client.PayBillAsync(AccountId, (double)Amount, "Hot-" + RechargeId.ToString()).Result;
                result.Result = response;
                result.ReplyMessage = response.Return_message;
                result.ReplyCode = response.IsSuccessful() ? (int)ReplyCode.Success : (int)ReplyCode.FailedNetworkPrepaidPlatform;

                if (response.Return_description.ToLower().Contains("task was canceled")) throw new Exception(response.Return_description);

                if (response.Return_description.ToLower().Contains("sufficient")) result.ReplyMessage = "Provider Error Code: 36180";

            }
            catch (Exception ex)
            {
                result.ReplyCode = (int)ReplyCode.FailedWebException;
                result.ReplyMessage = ex.Message;
            }
            return result;
        }

        [WebMethod]
        public RechargeVoipResponse RechargeAccountVoip(string AppKey, string AccountId, decimal Amount, int RechargeId)
        {
            if (AppKey != EconetAppKey) return
                   new RechargeVoipResponse()
                   {
                       ReplyMessage = "Login Failed with AppKey",
                       ReplyCode = (int)ReplyCode.FailedWebLogin
                   };

            if (TeloneAPIDisabled) return
                    new RechargeVoipResponse()
                    {
                        ReplyMessage = "Provider Error Code: 36188",
                        ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform
                    }; ;

            var result = new RechargeVoipResponse();
            var client = new TelOneClient(baseUrl);

            try
            {
                var response = client.RechargeVoipAsync(AccountId, (double)Amount, "Hot-" + RechargeId.ToString()).Result;
                result.Result = response;
                result.ReplyMessage = response.ResponseDescription;
                result.ReplyCode = response.IsSuccessful() ? (int)ReplyCode.Success : (int)ReplyCode.FailedNetworkPrepaidPlatform;

                if (response.ResponseDescription.ToLower().Contains("task was canceled")) throw new Exception(response.ResponseDescription);

                if (response.ResponseDescription.ToLower().Contains("sufficient")) result.ReplyMessage = "Provider Error Code: 36180";

            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("task was canceled")) throw new Exception(ex.Message);
                if (ex.Message.ToLower().Contains("sufficient")) result.ReplyMessage = "Provider Error Code: 36180";
                result.ReplyCode = (int)ReplyCode.FailedWebException;
                result.ReplyMessage = ex.Message;
            }
            return result;
        }


    }

    public static class TeloneExtensions
    {

        public static bool IsSuccessful(this PurchaseBroadbandProductsResponse result) => TransactionSuccessful(result.ResponseCode, result.ResponseDescription);
        public static bool IsSuccessful(this CustomerAccountResponse result) => TransactionSuccessful(result.ResponseCode, result.ResponseDescription);
        public static bool IsSuccessful(this VoipRechargeResponse result) => TransactionSuccessful(result.ResponseCode, result.ResponseDescription);
        public static bool IsSuccessful(this QueryCustomerVoiceBalanceResponse result) => TransactionSuccessful(result.ResultHeaderField.ResultCodeField);
        public static bool IsSuccessful(this RechargeAdslAccountResponse result) => TransactionSuccessful(result.ResponseCode, result.ResponseDescription);
        public static bool IsSuccessful(this PayBillResponse result) => TransactionSuccessful(result.Return_code.ToString(), result.Return_description);
        public static bool TransactionSuccessful(string ResponseCode, string ResponseDescription = "")
        {
            try
            {
                if (Convert.ToInt32(ResponseCode) == 0) return true;
            }
            catch (Exception)
            { }
            if (ResponseDescription.ToLower().Contains("successfully")) return true;

            return false;
        }

        public static int GetTotalAmountField(this QueryCustomerVoiceBalanceResponse response)
        {
            return response.QueryBalanceResultField.FirstOrDefault().BalanceResultField.FirstOrDefault().TotalAmountField;
        }
    }

}

namespace Hot.Web.Provider.Interfaces.Models.TelOne
{
    public class CustomerVoiceBalanceResponse : BaseResponse
    {
        public decimal Balance { get; set; }
        public QueryCustomerVoiceBalanceResponse Result { get; set; }
    }

    public class RechargeVoipResponse : BaseResponse
    {
        public VoipRechargeResponse Result { get; set; }
    }

    public class PayAccountBillResponse : BaseResponse
    {
        public PayBillResponse Result { get; set; }
    }

    public class AccountBalanceResponse : BaseResponse
    {
        public decimal Balance { get; set; }
        public List<AccountBalance> Result { get; set; }
    }

    public class VerifyAccountResponse : BaseResponse
    {
        public CustomerAccountResponse Result { get; set; }
    }

    public class RechargeAccountResponse : BaseResponse
    {
        public RechargeAdslAccountResponse Result { get; set; }
    }

    public class BulkPurchaseBroadbandResponse : BaseResponse
    {
        public PurchaseBroadbandProductsResponse Result { get; set; }
    }

    public class GetBundleResponse : BaseResponse
    {
        public List<BroadbandProductItem> List { get; set; }
    }

    public class BroadbandProductItem
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public static implicit operator BroadbandProductItem(BroadbandProduct item)
        {
            return new BroadbandProductItem()
            {
                ProductId = item.ProductId
                ,
                Name = item.ProductName
                ,
                Description = item.ProductDescription
                ,
                Price = (decimal)(item.ProductPrices.FirstOrDefault(p => p.Currency.ToUpper().Trim() == "ZWG") ?? new ProductPrice()).Price
            };
        }
        public static BroadbandProductItem MapUSD(BroadbandProduct item)
        {
            return new BroadbandProductItem()
            {
                ProductId = item.ProductId
                ,
                Name = item.ProductName
                ,
                Description = item.ProductDescription
                ,
                Price = (decimal)(item.ProductPrices.FirstOrDefault(p => p.Currency == "USD") ?? new ProductPrice()).Price
            };
        }
    }

}