//using Hot.EconetBundle.Models;
//using Hot.EconetBundle;
using Hot.Web.Provider.Interfaces.EPayGateway;
using Hot.Web.Provider.Interfaces.Services.EconetPrepaidGW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;
using System.Configuration;
using Hot.EconetBundle;
using Hot.API.Interface.Models;
using Hot.API.Interface;
using static Hot.Web.Provider.Interfaces.ZESA;

namespace Hot.Web.Provider.Interfaces
{
    /// <summary>
    /// Summary description for Econet
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Econet : System.Web.Services.WebService
    {
        private const string ServiceId = "43";
        private const string ServiceProviderId = "Mobile-Connectivity";
        static readonly bool UsePostPaid = Convert.ToBoolean(ConfigurationManager.AppSettings["EconetUsePostPaid"]);
        static readonly bool EconetZWLDisabled = Convert.ToBoolean(ConfigurationManager.AppSettings["EconetDisabled"]);
        static readonly string BasePrepaidURL = ConfigurationManager.AppSettings["EconetPrepaidURL"];

        [WebMethod]
        public CreditResponse RechargeMobile(string TargetMobile, decimal Amount, string Reference)
        {
            if (EconetZWLDisabled) return FailedTransaction(TargetMobile, Amount, Reference);
            return !UsePostPaid
                ? PrepaidRecharge(ref TargetMobile, Amount, Reference)
             : PostPaidRecharge(TargetMobile, Amount, Reference);
        }

        private CreditResponse FailedTransaction(string TargetMobile, decimal Amount, string Reference)
        {
            return new CreditResponse
            {
                responseCode =
                         ((int)StatusCodes.General_Error).ToString(),
                narrative = "Provider Platform Error: 36180",
                creditReq = GetcreditReq(TargetMobile, Amount, Reference)
            };
        }

        private static CreditResponse PostPaidRecharge(string TargetMobile, decimal Amount, string Reference)
        {
            var response = new CreditResponse();
            try
            {
                response = new econetvasClient().creditSubscriber(new CreditRequest()
                {
                    serviceId = ServiceId,
                    amount = (double)Amount,
                    sourceMobileNumber = TargetMobile,
                    targetMobileNumber = TargetMobile,
                    reference = Reference,
                    serviceProviderId = ServiceProviderId,
                    numberOfDays = 90,
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        private static CreditResponse PrepaidRecharge(ref string TargetMobile, decimal Amount, string Reference)
        {
            try
            {

                TargetMobile = (TargetMobile.StartsWith("0") ? ("263" + TargetMobile.Substring(1)) : TargetMobile);
                var client = new Client(BasePrepaidURL);
                var result = client.RechargeAirtimeZwlAsync(TargetMobile, Amount.ToString(), Reference).Result;
                try
                {
                    GetResultBalance(result, Amount);
                }
                catch (Exception)
                {
                }

                return new CreditResponse
                {
                    responseCode = result.StatusCode == 0 ? "000" : result.StatusCode.ToString(),
                    narrative = result.StatusCode == 0 ? $"Credit Successful#Prepaid#{result.InitialWalletBalance},{result.FinalWalletBalance}" : HandleError(result),
                    creditReq = GetcreditReq(TargetMobile, Amount, Reference),

                };

            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("insufficient"))
                {
                    return new CreditResponse
                    {
                        responseCode = ((ex.Message.StartsWith("Invalid Bundle") ?
                       (int)StatusCodes.Invalid_Bundle_Quantity :
                       (int)StatusCodes.General_Error)).ToString(),
                        narrative = "Provider Error Code: 36180",
                        creditReq = GetcreditReq(TargetMobile, Amount, Reference) 
                    };

                }
                return new CreditResponse
                {
                    responseCode = ((ex.Message.StartsWith("Invalid Bundle") ?
                        (int)StatusCodes.Invalid_Bundle_Quantity :
                        (int)StatusCodes.General_Error)).ToString(),
                    narrative = ex.Message,
                    creditReq = GetcreditReq(TargetMobile, Amount, Reference)
                };
            }
        }

        private static string HandleError(LoadAirtimeResponse result)
        {
            var resultDescription = result.Description;
            if (result.StatusCode == 906 || result.Description.ToLower().Contains("transaction limits"))
            {
                resultDescription = "Provider Error Code: 36180";
            }
            return resultDescription;
        }

        private static CreditRequest GetcreditReq(string TargetMobile, decimal Amount, string Reference)
        {
            return new CreditRequest() { reference = Reference, amount = (double)Amount, targetMobileNumber = TargetMobile, serviceProviderId = ServiceProviderId, serviceId = ServiceProviderId };
        }

        [WebMethod]
        public DebitResponse DebitMobile(string TargetMobile, decimal Amount, string Reference)
        {
            var response = new DebitResponse();
            try
            {
                response = new econetvasClient().debitSubscriber(new DebitRequest()
                {
                    serviceId = ServiceId,
                    amount = (double)-Amount,
                    sourceMobileNumber = TargetMobile,
                    targetMobileNumber = TargetMobile,
                    reference = Reference,
                    serviceProviderId = ServiceProviderId,
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        [WebMethod]
        public BalanceResponse EndUserBalance(string TargetMobile, string Reference)
        {
            //if (!UsePostPaid) return new BalanceResponse()
            //{
            //    narrative = "EndUser Balance unavailable",
            //    currentBalance = 0,
            //    responseCode = "999"
            //};

            try
            {
                return new econetvasClient().balanceEnquiry(new BalanceRequest()
                {
                    serviceId = ServiceId,
                    mobileNumber = TargetMobile,
                    reference = Reference,
                    serviceProviderId = ServiceProviderId,
                    dedicatedAccount = 1
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [WebMethod]
        public BalanceResponse EndUserBalanceUSD(string TargetMobile, string Reference)
        {
            if (!UsePostPaid) return new BalanceResponse()
            {
                narrative = "EndUser Balance unavailable",
                currentBalance = 0,
                responseCode = "999"
            };

            try
            {
                return new econetvasClient().balanceEnquiry(new BalanceRequest()
                {
                    serviceId = ServiceId,
                    mobileNumber = TargetMobile,
                    reference = Reference,
                    serviceProviderId = ServiceProviderId,
                    dedicatedAccount = 398
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public CreditReversalResult RechargeReversal(string TargetMobile, decimal Amount, string Reference)
        {
            var response = new CreditReversalResult();
            try
            {
                response = new econetvasClient().creditReversal(new CreditRequest()
                {
                    serviceId = ServiceId,
                    amount = (double)Amount,
                    sourceMobileNumber = TargetMobile,
                    targetMobileNumber = TargetMobile,
                    reference = Reference,
                    serviceProviderId = ServiceProviderId,
                    numberOfDays = 90,
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }


        private static void GetResultBalance(LoadAirtimeResponse result, decimal Amount)
        {
            try
            {
                var client = new Client(BasePrepaidURL);
                var balances = client.AccountBalanceZwlAsync().Result;
                var balance = balances.AccountBalances.First();

                if (result.Status == 1)
                {
                    result.InitialWalletBalance = (double)((balance.Amount + (Amount * 100)) / 100);
                    result.FinalWalletBalance = (double)((decimal)balance.Amount / 100);
                }
                else
                {
                    result.InitialWalletBalance = (double)((decimal)balance.Amount / 100);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
