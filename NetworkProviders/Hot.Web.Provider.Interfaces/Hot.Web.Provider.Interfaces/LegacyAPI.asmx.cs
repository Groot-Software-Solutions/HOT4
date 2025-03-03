using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Hot.API.Interface;
using Hot.API.Interface.Models;

namespace Hot.Web.Provider.Interfaces
{
    /// <summary>
    /// Summary description for OldtoNewAPI
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class LegacyAPI : System.Web.Services.WebService
    {
        public const string BaseUrl = "http://127.0.0.1:5000/api/v1/";

        [WebMethod]
        public ReturnObject HOTBalance(string AccessCode, string AccessPassword, string EmptyString) 
        {
            try
            {
                WalletBalanceResponse response = (new HotAPIClient(BaseUrl, AccessCode, AccessPassword)).GetWalletBalanceAsync().Result; 
                return new ReturnObject
                {
                    ReturnCode = (response.ReplyCode==2 ? 1 :-1),
                    ReturnMsg = (response.ReplyCode == 2 ? 302 : response.ReplyCode).ToString()+","+response.ReplyMsg,
                    ReturnValue = response.WalletBalance
                };

            }
            catch (Exception ex)
            {
                return new ReturnObject
                {
                    ReturnCode = -1,
                    ReturnMsg = ex.Message,
                    ReturnValue = 0
                };
            }
            

        }

        [WebMethod]
        public ReturnObject HOTPhoneBalance(string AccessCode, string AccessPassword,string TargetMobile)
        {
            try
            {
                EndUserBalanceResponse response = (new HotAPIClient(BaseUrl, AccessCode, AccessPassword)).GetEndUserBalance(TargetMobile);
                return new ReturnObject
                {
                    ReturnCode = (response.ReplyCode == 2 ? 1 : -1),
                    ReturnMsg = response.ReplyMsg,
                    ReturnValue = Convert.ToDecimal(response.MobileBalance)
                };

            }
            catch (Exception ex)
            {
                return new ReturnObject
                {
                    ReturnCode = -1,
                    ReturnMsg = ex.Message,
                    ReturnValue = 0
                };
            }
        }

        [WebMethod]
        public ReturnObject HOTRecharge(string AccessCode, string AccessPassword, string TargetMobile, decimal Amount)
        {
            try
            {
                string Reference = "Legacy-"+Guid.NewGuid().ToString(); 
                RechargeResponse response = (new HotAPIClient(BaseUrl, AccessCode, AccessPassword))
                    .Recharge(TargetMobile,Amount,Reference);
                return new ReturnObject
                {
                    ReturnCode = (response.ReplyCode == 2 ? 1 : -1),
                    ReturnMsg = (response.ReplyCode == 2 ? 303 : response.ReplyCode).ToString() + "," + response.ReplyMsg,
                    ReturnValue = Convert.ToDecimal(response.FinalBalance)
                };

            }
            catch (Exception ex)
            {
                return new ReturnObject
                {
                    ReturnCode = -1,
                    ReturnMsg = ex.Message,
                    ReturnValue = 0
                };
            }
        }

        [WebMethod]
        public ReturnObject RetailRecharge(string AccessCode, string AccessPAssword, string TargetMobile, decimal Amount)
        {
            try
            {
                string Reference = "Legacy-" + Guid.NewGuid().ToString();
                string CustomSMS = "%COMPANY% topped up your Airtime with $%AMOUNT%." + Environment.NewLine
                    + "Your new balance is $%BALANCE%.";
                RechargeResponse response = (new HotAPIClient(BaseUrl, AccessCode, AccessPAssword))
                    .Recharge(TargetMobile, Amount, Reference, CustomSMS);
                return new ReturnObject
                {
                    ReturnCode = (response.ReplyCode == 2 ? 1 : -1),
                    ReturnMsg = (response.ReplyCode == 2 ? 303 : response.ReplyCode).ToString() + "," + response.ReplyMsg,
                    ReturnValue = Convert.ToDecimal(response.FinalBalance)
                };

            }
            catch (Exception ex)
            {
                return new ReturnObject
                {
                    ReturnCode = -1,
                    ReturnMsg = ex.Message,
                    ReturnValue = 0
                };
            }
        }
        
        [WebMethod]
        public ReturnObject HOTRechargeReversal (string AccessCode, string AccessPassword, string OldRechargeID )
        {
            return 
                new ReturnObject
                {
                    ReturnCode = (int)ReplyCode.FailedWebException,
                    ReturnMsg = "Method Not Implemented",
                    ReturnValue = 0
                };
        }

        [WebMethod]
        public ReturnObject xEcoCashPayment(string AccessCode, string AccessPassword, 
            DateTime TrxDateTime,decimal TrxAmount,string MSISDN,string CustomerAccount,string SenderName,
            string RetrievalReference,string SystemsAuditTrace, string EcoCashReference)
        {
       
            return
                new ReturnObject
                {
                    ReturnCode = (int)ReplyCode.FailedWebException,
                    ReturnMsg = "Method Not Implemented",
                    ReturnValue = 0
                };
        }

    }
    
}
