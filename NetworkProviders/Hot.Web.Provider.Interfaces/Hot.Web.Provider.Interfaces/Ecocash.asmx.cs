using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Services;
using Hot.Web.Provider.Interfaces.Services.Ecocash;

namespace Hot.Web.Provider.Interfaces
{
    /// <summary>
    /// Summary description for Ecocash
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Ecocash : System.Web.Services.WebService
    {
        public static string EconetAppKey = ConfigurationManager.AppSettings["EconetAppKey"];
        private readonly string baseUrl = ConfigurationManager.AppSettings["EcocashAPIBaseUrl"];
        private readonly bool APIDisabled = Convert.ToBoolean(ConfigurationManager.AppSettings["EcocashDisabled"]);
        
        [WebMethod]
        public EcocashResult Charge( string AppKey,int account , string mobileNumber, string reference, decimal amount,string OnBehalfOf)
        {
            if (AppKey != EconetAppKey) return
                    new EcocashResult
                    {
                         ErrorData = "Login Failed with AppKey",
                         ValidResponse = false
                    };
            if (APIDisabled) return
                    new EcocashResult
                    {
                        ErrorData = "Provider Error Code: 36188", 
                        ValidResponse = false
                    };

            try
            {
                var client = new EcocashClient(baseUrl);
                var response = client.ChargeOnBehalfWithRemarkAsync((Accounts)account ,mobileNumber,reference,(double)amount,"Hot Recharge",OnBehalfOf).Result;
                return response;
            }
            catch (Exception ex)
            {
                return new EcocashResult
                {
                    ErrorData = ex.Message,
                    ValidResponse = false
                };
            }
             
        }

        [WebMethod]
        public EcocashResult Refund(string AppKey, int account, string mobileNumber, string reference,decimal amount, string ecocashReference)
        {
            if (AppKey != EconetAppKey) return
                    new EcocashResult
                    {
                        ErrorData = "Login Failed with AppKey",
                        ValidResponse = false
                    };
            if (APIDisabled) return
                    new EcocashResult
                    {
                        ErrorData = "Provider Error Code: 36188",
                        ValidResponse = false
                    };

            try
            {
                var client = new EcocashClient( baseUrl );
                var response = client.RefundAsync((Accounts)account, mobileNumber, reference, (double)amount,ecocashReference).Result;
                return response;
            }
            catch (Exception ex)
            {
                return new EcocashResult
                {
                    ErrorData = ex.Message,
                    ValidResponse = false
                };
            }

        }

        [WebMethod]
        public EcocashResult Query(string AppKey, int account, string mobileNumber, string reference)
        {
            if (AppKey != EconetAppKey) return
                    new EcocashResult
                    {
                        ErrorData = "Login Failed with AppKey",
                        ValidResponse = false
                    };
            if (APIDisabled) return
                    new EcocashResult
                    {
                        ErrorData = "Provider Error Code: 36188",
                        ValidResponse = false
                    };

            try
            {
                var client = new EcocashClient(baseUrl );
                var response = client.QueryAsync((Accounts)account, mobileNumber, reference ).Result;
                return response;
            }
            catch (Exception ex)
            {
                return new EcocashResult
                {
                    ErrorData = ex.Message,
                    ValidResponse = false
                };
            }

        }

        [WebMethod]
        public EcocashResult List(string AppKey, int account, string mobileNumber)
        {
            if (AppKey != EconetAppKey) return
                    new EcocashResult
                    {
                        ErrorData = "Login Failed with AppKey",
                        ValidResponse = false
                    };
            if (APIDisabled) return
                    new EcocashResult
                    {
                        ErrorData = "Provider Error Code: 36188",
                        ValidResponse = false
                    };

            try
            {
                var client = new EcocashClient( baseUrl);
                var response = client.ListAsync((Accounts)account, mobileNumber).Result;
                return response;
            }
            catch (Exception ex)
            {
                return new EcocashResult
                {
                    ErrorData = ex.Message,
                    ValidResponse = false
                };
            }

        }


    }
}
