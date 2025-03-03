using Hot.API.Interface;
using Hot.Web.Provider.Interfaces.Models;
using Hot.Web.Provider.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Services;

namespace Hot.Web.Provider.Interfaces
{
    /// <summary>
    /// Summary description for ZESA
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public partial class ZESA : System.Web.Services.WebService
    {
        public static string EconetAppKey = ConfigurationManager.AppSettings["EconetAppKey"];
        private readonly string baseUrl = ConfigurationManager.AppSettings["TokenProviderAPIBaseUrl"];
        private readonly bool TokenProviderDisabled = Convert.ToBoolean(ConfigurationManager.AppSettings["TokenProviderDisabled"]);

        [WebMethod]
        public PurchaseTokenResponse PurchaseZESAToken(string AppKey, string MeterNumber, decimal Amount, string Reference)
        {
            if (AppKey != EconetAppKey) return
                 new PurchaseTokenResponse
                 {
                     ReplyMessage = "Login Failed with AppKey",
                     ReplyCode = (int)ReplyCode.FailedWebLogin
                 };
            if (TokenProviderDisabled) return
                    new PurchaseTokenResponse
                    {
                        ReplyMessage = "Provider Error Code: 36188",
                        ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform
                    };

            var result = new PurchaseTokenResponse();
            var client = new ZesaClient(baseUrl);

            try
            {
                var balanceresponse = client.GetBalanceAsync().Result;
                result.InitialBalance = Convert.ToDecimal(balanceresponse.Balance);
            }
            catch
            {
                result.InitialBalance = 0;
            }


            try
            {
                result.CustomerInfo = client.GetCustomerInfoAsync(MeterNumber).Result;
                var response = client.PurchaseTokenAsync((double?)Amount, MeterNumber, Reference).Result;
                result.PurchaseToken = response;
                result.ReplyMessage = response.Narrative;
                result.ReplyCode = (int)ResponseCodeToReplyCode((ResponseCodes)Convert.ToInt32(result.PurchaseToken.ResponseCode ?? "05"));

            }
            catch (Exception ex)
            {
                result.ReplyMessage = ex.Message;
                result.ReplyCode = (int)ReplyCode.FailedWebException;
            }

            return result;
        }
        [WebMethod]
        public PurchaseTokenResponse PurchaseZESATokenByCurrency(string AppKey, string MeterNumber, decimal Amount, string Reference, string Currency = "ZWG")
        {
            if (AppKey != EconetAppKey) return
                 new PurchaseTokenResponse
                 {
                     ReplyMessage = "Login Failed with AppKey",
                     ReplyCode = (int)ReplyCode.FailedWebLogin
                 };
            if (TokenProviderDisabled) return
                    new PurchaseTokenResponse
                    {
                        ReplyMessage = "Provider Error Code: 36188",
                        ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform
                    };

            var result = new PurchaseTokenResponse();
            var client = new ZesaClient(baseUrl);




            try
            {
                result.CustomerInfo = client.GetCustomerInfoAsync(MeterNumber).Result;
                if (result.CustomerInfo.Currency.ToUpper() != Currency.ToUpper() && Currency.ToUpper() != "USD") return new PurchaseTokenResponse
                {
                    ReplyMessage = $"The meter is a {result.CustomerInfo.Currency} account. Please try the transaction again with the correct currency.",
                    ReplyCode = (int)ReplyCode.FailedPinDenomination
                };

                try
                {
                    var balanceresponse = Currency.ToUpper() == "ZWG" ? client.GetBalanceAsync().Result : client.GetBalanceUSDAsync().Result;
                    result.InitialBalance = Convert.ToDecimal(balanceresponse.Balance);
                }
                catch
                {
                    result.InitialBalance = 0; 
                }

                var response = Currency.ToUpper() != "USD"
                    ? client.PurchaseTokenAsync((double?)Amount, MeterNumber, Reference).Result
                    : client.PurchaseTokenUSDAsync((double?)Amount, MeterNumber, Reference).Result;

                result.PurchaseToken = response;
                result.ReplyMessage = response.Narrative;
                if (result.ReplyMessage.Contains("insufficient")) result.ReplyMessage = "Provider Error Code: 36180"; 
                result.ReplyCode = (int)ResponseCodeToReplyCode((ResponseCodes)Convert.ToInt32(result.PurchaseToken.ResponseCode ?? "05"));
                
                result.FinalBalance = result.InitialBalance - (result.ReplyCode == (int)ReplyCode.Success ? Amount : 0);
            }
            catch (Exception ex)
            {
                result.ReplyMessage = ex.Message;
                result.ReplyCode = (int)ReplyCode.FailedWebException;
            }

            return result;
        }

        [WebMethod]
        public CustomerInfoResponse GetCustomerInfo(string AppKey, string MeterNumber)
        {
            if (AppKey != EconetAppKey) return
                   new CustomerInfoResponse
                   {
                       ReplyMessage = "Login Failed with AppKey",
                       ReplyCode = (int)ReplyCode.FailedWebLogin
                   };
            if (TokenProviderDisabled) return
                    new CustomerInfoResponse
                    {
                        ReplyMessage = "Provider Error Code: 36188",
                        ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform
                    };

            var result = new CustomerInfoResponse();
            try
            {
                var client = new ZesaClient(baseUrl);
                var response = client.GetCustomerInfoAsync(MeterNumber).Result;
                result.CustomerInfo = response;
                result.ReplyMessage = "Successfully retreived customer data";
                result.ReplyCode = (int)ReplyCode.Success;

            }
            catch (Exception ex)
            {
                result.ReplyMessage = ex.Message;
                result.ReplyCode = (int)ReplyCode.FailedWebException;
            }

            return result;
        }

        [WebMethod]
        public VendorBalanceResponse GetBalance(string AppKey, string Currency = "")
        {
            if (AppKey != EconetAppKey) return
                    new VendorBalanceResponse
                    {
                        ReplyMessage = "Login Failed with AppKey",
                        ReplyCode = (int)ReplyCode.FailedWebLogin
                    };
            if (TokenProviderDisabled) return
                    new VendorBalanceResponse
                    {
                        ReplyMessage = "Provider Error Code: 36188",
                        ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform
                    };

            var result = new VendorBalanceResponse();
            try
            {
                var client = new ZesaClient(baseUrl);
                var response = Currency.ToUpper() == "USD"
                    ? client.GetBalanceUSDAsync().Result
                    : client.GetBalanceAsync().Result;
                result.VendorBalance = response;
                result.ReplyMessage = "Successfully retreived vendor balance data";
                result.ReplyCode = (int)ReplyCode.Success;
            }
            catch (Exception ex)
            {
                result.ReplyMessage = ex.Message;
                result.ReplyCode = (int)ReplyCode.FailedWebException;
            }

            return result;
        }


        public class CustomerInfoResponse : BaseResponse
        {
            public CustomerInfo CustomerInfo { get; set; }
        }
        public class PurchaseTokenResponse : BaseResponse
        {
            public decimal InitialBalance;
            public decimal FinalBalance;
            public PurchaseToken PurchaseToken;
            public CustomerInfo CustomerInfo;
        }
        public class VendorBalanceResponse : BaseResponse
        {
            public VendorBalance VendorBalance { get; set; }
        }
        public enum ResponseCodes
        {
            TransactionSuccessful = 0,
            GeneralError = 5,
            TransactionInProgress = 9,
            AmountOutsideRange = 12,
            InvalidMeterNumber = 14,
            InsufficientFunds = 51,
            AccountPendingReversal = 57,
            SecurityViolation = 63,
            TransactionTimeout = 68,
            DuplicateTransaction = 94,

        }
        private static ReplyCode ResponseCodeToReplyCode(ResponseCodes code)
        {
            switch (code)
            {
                case ResponseCodes.TransactionSuccessful:
                    return ReplyCode.Success;

                case ResponseCodes.AmountOutsideRange:
                    return ReplyCode.FailedRechargeAmountLimits;

                case ResponseCodes.DuplicateTransaction:
                    return ReplyCode.FailedDuplicate;

                case ResponseCodes.InsufficientFunds:
                    return ReplyCode.FailedInsufficeintWalletBalance;

                case ResponseCodes.InvalidMeterNumber:
                    return ReplyCode.FailedBalanceRequest;

                case ResponseCodes.TransactionInProgress:
                    return ReplyCode.SuccessRechargeReceived;

                case ResponseCodes.TransactionTimeout:
                    return ReplyCode.NetworkTimeout;

                case ResponseCodes.SecurityViolation:
                    return ReplyCode.NetworkWebserviceError;

                case ResponseCodes.GeneralError:
                    return ReplyCode.NetworkGeneralError;

                case ResponseCodes.AccountPendingReversal:
                    return ReplyCode.SuccessfulReversalCustomer;
                default:
                    return ReplyCode.NetworkWebserviceError;
            }
        }

    }
}
