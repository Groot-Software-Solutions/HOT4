using Hot.Web.Provider.Interfaces.Services.Ecocash;
using Hot.Web.Provider.Interfaces.Services.Nyaradzao;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Hot.Web.Provider.Interfaces
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Nyaradzo : System.Web.Services.WebService
    { 
        public static string EconetAppKey = ConfigurationManager.AppSettings["EconetAppKey"];
        private readonly string baseUrl = ConfigurationManager.AppSettings["NyaradzoAPIBaseUrl"];
        private readonly bool APIDisabled = Convert.ToBoolean(ConfigurationManager.AppSettings["NyaradzoDisabled"]);

        [WebMethod]
        public NyaradzoResultModel ProcessPayment(string AppKey, string PolicyNumber, string reference, decimal amount)
        {
            if (AppKey != EconetAppKey) return
                    new NyaradzoResultModel
                    {
                        ErrorData = "Login Failed with AppKey",
                        ValidResponse = false
                    };
            if (APIDisabled) return
                    new NyaradzoResultModel
                    {
                        ErrorData = "Provider Error Code: 36188",
                        ValidResponse = false
                    };
            try
            {
                var client = new NyaradzoClient(baseUrl);
                var response = client.PaymentProcessingAsync(
                    new PaymentProcessingRequest()
                    {
                        AmountPaid = (double)amount,
                        Date = DateTimeOffset.Now,
                        PolicyNumber = PolicyNumber,
                        Reference = reference,
                        NumberOfMonthsPaid = 1,
                        MonthlyPremium = (double)amount,
                         Currency = Currency._1
                    }
                    ).Result;
                if (response.ValidResponse)
                {
                    try
                    {
                        var account = AccountQuery(AppKey, PolicyNumber); 
                        response.Item = account.Item;
                    }
                    catch (Exception)
                    {
                         
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return new NyaradzoResultModel
                {
                    ErrorData = ex.Message,
                    ValidResponse = false
                };
            }
        }

        [WebMethod]
        public NyaradzoResultModel Refund(string AppKey, string reference)
        {
            if (AppKey != EconetAppKey) return
                    new NyaradzoResultModel
                    {
                        ErrorData = "Login Failed with AppKey",
                        ValidResponse = false
                    };
            if (APIDisabled) return
                    new NyaradzoResultModel
                    {
                        ErrorData = "Provider Error Code: 36188",
                        ValidResponse = false
                    };

            try
            {
                var client = new NyaradzoClient(baseUrl);
                var response = client.ReversalAsync(new ReversalRequest()
                {
                    Reference = reference
                }).Result;
                return response;
            }
            catch (Exception ex)
            {
                return new NyaradzoResultModel
                {
                    ErrorData = ex.Message,
                    ValidResponse = false
                };
            }

        }

        [WebMethod]
        public NyaradzoResultModel AccountQuery(string AppKey, string PolicyNumber)
        {
            if (AppKey != EconetAppKey) return
                    new NyaradzoResultModel
                    {
                        ErrorData = "Login Failed with AppKey",
                        ValidResponse = false
                    };
            if (APIDisabled) return
                    new NyaradzoResultModel
                    {
                        ErrorData = "Provider Error Code: 36188",
                        ValidResponse = false
                    };

            try
            {
                var client = new NyaradzoClient(baseUrl);
                var response = client.AccountEnquiryAsync(PolicyNumber, Guid.NewGuid().ToString()).Result;
                return response;
            }
            catch (Exception ex)
            {
                return new NyaradzoResultModel
                {
                    ErrorData = ex.Message,
                    ValidResponse = false
                };
            }

        }

    }
}
