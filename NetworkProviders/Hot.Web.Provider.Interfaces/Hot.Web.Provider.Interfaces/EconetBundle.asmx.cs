using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using Hot.EconetBundle;
using Hot.EconetBundle.Models;
using Hot.Web.Provider.Interfaces.Services;
using Hot.Web.Provider.Interfaces.Services.EconetPrepaidGW;
using LoadAirtimeResponse = Hot.Web.Provider.Interfaces.Services.EconetPrepaidGW.LoadAirtimeResponse;
using LoadDataResponse = Hot.Web.Provider.Interfaces.Services.EconetPrepaidGW.LoadDataResponse;

namespace Hot.Web.Provider.Interfaces
{
    /// <summary>
    /// Summary description for EconetBundle
    /// </summary>
    [WebService(Namespace = "http://hot.co.zw:8007/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class EconetBundle : System.Web.Services.WebService
    {
        static readonly string _ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
        public static string EconetAppKey = ConfigurationManager.AppSettings["EconetAppKey"];
        private static List<Hot.EconetBundle.Models.BundleProduct> bundles = new List<BundleProduct>();
        private static Dictionary<string, Hot.EconetBundle.Models.BundleProduct> bundleList = new Dictionary<string, Hot.EconetBundle.Models.BundleProduct>();
        static readonly bool EconetBundleAPIDisabled = Convert.ToBoolean(ConfigurationManager.AppSettings["EconetBundleAPIDisabled"]);
        private const string USDUsername = "CommXmlUSD";
        private const string USDPassword = "pass123";
        private const string ZWLUsername = "CommShopXML_User";
        private const string ZWLPassword = "P@553d";
        static readonly string BasePrepaidURL = ConfigurationManager.AppSettings["EconetPrepaidURL"];


        [WebMethod]
        public LoadDataResponse Recharge(string AppKey, string TargetMobile, string BundleCode, long RechargeId, string Currency = "")
        {
            if (AppKey != EconetAppKey) return new LoadDataResponse
            {
                StatusCode = (int)StatusCodes.Download_Encryption_Error,
                Description = "Login Failed with AppKey"
            };
            if (EconetBundleAPIDisabled) return
                   new LoadDataResponse
                   {
                       StatusCode = (int)StatusCodes.General_Error,
                       Description = "Provider Error Code: 36188"
                   };
            try
            {
                TargetMobile = (TargetMobile.StartsWith("0") ? ("263" + TargetMobile.Substring(1)) : TargetMobile);
                Hot.EconetBundle.Models.BundleProduct bundle = GetBundle(BundleCode);
                System.Threading.Thread.Sleep((new Random()).Next(53, 489)); // Random sleep to fix Telecel Concurrency issue
                if (Currency == "1")
                {
                    var client = new Client(BasePrepaidURL);
                    var result = client.RechargeDataUsdAsync(TargetMobile, ((decimal)bundle.Amount / (decimal)100).ToString(), bundle.ProductCode, RechargeId.ToString()).Result;
                    GetResultBalance(result, bundle.Amount, Currency);
                    return result;
                }
                else
                {
                    var client = new Client(BasePrepaidURL);
                    var result = client.RechargeDataZwlAsync(TargetMobile, ((decimal)bundle.Amount / (decimal)100).ToString(), bundle.ProductCode, RechargeId.ToString()).Result;
                    GetResultBalance(result, bundle.Amount, Currency);
                    return result;
                }

            }
            catch (Exception ex)
            {
                return new LoadDataResponse
                {
                    StatusCode = (ex.Message.StartsWith("Invalid Bundle") ?
                        (int)StatusCodes.Invalid_Bundle_Quantity :
                        (int)StatusCodes.General_Error),
                    Description = ex.Message
                };
            }
        }
        //public LoadDataResponse Recharge(string AppKey, string TargetMobile, string BundleCode, long RechargeId, string Currency = "")
        //{
        //    if (AppKey != EconetAppKey) return new LoadDataResponse
        //    {
        //        StatusCode = (int)StatusCodes.Download_Encryption_Error,
        //        Description = "Login Failed with AppKey"
        //    };
        //    if (EconetBundleAPIDisabled) return
        //           new LoadDataResponse
        //           {
        //               StatusCode = (int)StatusCodes.General_Error,
        //               Description = "Provider Error Code: 36188"
        //           };
        //    try
        //    {
        //        TargetMobile = (TargetMobile.StartsWith("0") ? ("263" + TargetMobile.Substring(1)) : TargetMobile);
        //        Hot.EconetBundle.Models.BundleProduct bundle = GetBundle(BundleCode);
        //        System.Threading.Thread.Sleep((new Random()).Next(53, 489)); // Random sleep to fix Telecel Concurrency issue
        //        if (Currency == "1")
        //        {

        //            var result = BundleAPIClient.LoadDataBundle(
        //                    TargetMobile,
        //                    bundle.Amount,
        //                    bundle.ProductCode,
        //                    RechargeId.ToString(),
        //                    BundleAPIClient.URL, USDUsername, USDPassword, CurrencyCode.USD
        //                   );

        //            GetResultBalance(result, bundle.Amount, BundleAPIClient.URL, USDUsername, USDPassword);
        //            return result;
        //        }
        //        else
        //        {
        //            var result = BundleAPIClient.LoadDataBundle(
        //               TargetMobile,
        //               bundle.Amount,
        //               bundle.ProductCode,
        //               RechargeId.ToString(),
        //               BundleAPIClient.URL, ZWLUsername, ZWLPassword, CurrencyCode.RTGS
        //              );
        //            GetResultBalance(result, bundle.Amount, BundleAPIClient.URL, ZWLUsername, ZWLPassword);
        //            return result;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return new LoadDataResponse
        //        {
        //            StatusCode = (ex.Message.StartsWith("Invalid Bundle") ?
        //                (int)StatusCodes.Invalid_Bundle_Quantity :
        //                (int)StatusCodes.General_Error),
        //            Description = ex.Message
        //        };
        //    }
        //}
        [WebMethod]
        public LoadAirtimeResponse RechargeAirtime(string AppKey, string TargetMobile, decimal Amount, long RechargeId, string Currency = "")
        {
            if (AppKey != EconetAppKey) return new LoadAirtimeResponse
            {
                StatusCode = (int)StatusCodes.Download_Encryption_Error,
                Description = "Login Failed with AppKey"
            };
            if (EconetBundleAPIDisabled) return
                   new LoadAirtimeResponse
                   {
                       StatusCode = (int)StatusCodes.General_Error,
                       Description = "Provider Error Code: 36188"
                   };
            try
            {
                TargetMobile = (TargetMobile.StartsWith("0") ? ("263" + TargetMobile.Substring(1)) : TargetMobile);
                var client = new Client(BasePrepaidURL);
                if (Currency == "1")
                {
                    var result = client.RechargeAirtimeUsdAsync(TargetMobile, Amount.ToString(), RechargeId.ToString()).Result;
                    GetResultBalance(result, Amount, Currency);

                    return result;

                }
                else
                {
                    var result = client.RechargeAirtimeZwlAsync(TargetMobile, Amount.ToString(), RechargeId.ToString()).Result;
                    GetResultBalance(result, Amount, Currency);

                    return result;
                }

                //if (Currency == "1")
                //{
                //    var result = BundleAPIClient.LoadAirtime(
                //      TargetMobile,
                //      (int)(Amount * 100),
                //      RechargeId.ToString(),
                //      BundleAPIClient.URL, USDUsername, USDPassword, CurrencyCode.USD);
                //    GetResultBalance(result, Amount, BundleAPIClient.URL, USDUsername, USDPassword);

                //    return result;

                //}
                //else
                //{
                //    var result = BundleAPIClient.LoadAirtime(
                //      TargetMobile,
                //      (int)(Amount * 100),
                //      Guid.NewGuid().ToString(),
                //      BundleAPIClient.URL, ZWLUsername, ZWLPassword, CurrencyCode.RTGS);
                //    GetResultBalance(result, Amount, BundleAPIClient.URL, ZWLUsername, ZWLPassword);
                //    return result;
                //}

            }
            catch (Exception ex)
            {
                return new LoadAirtimeResponse
                {
                    StatusCode = (ex.Message.StartsWith("Invalid Bundle") ?
                        (int)StatusCodes.Invalid_Bundle_Quantity :
                        (int)StatusCodes.General_Error),
                    Description = ex.Message
                };
            }
        }

        private static void GetResultBalance(LoadAirtimeResponse result, decimal Amount, string Currency)
        {
            try
            {
                var client = new Client(BasePrepaidURL);
                var balances = Currency == "1" ? client.AccountBalanceUsdAsync().Result : client.AccountBalanceZwlAsync().Result;
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
            catch (Exception)
            {
            }
        }
        private static void GetResultBalance(LoadDataResponse result, decimal Amount, string Currency)
        {
            try
            {
                var client = new Client(BasePrepaidURL);
                var balances = Currency == "1" ? client.AccountBalanceUsdAsync().Result : client.AccountBalanceZwlAsync().Result;
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
            catch (Exception)
            {
            }
        }

        private static void GetResultBalance(LoadAirtimeResponse result, decimal Amount, string URL, string Username, string Password)
        {
            try
            {
                var balances = BundleAPIClient.AccountBalance(URL, Username, Password);
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
            catch (Exception)
            {
            }
        }
        private static void GetResultBalance(LoadDataResponse result, decimal Amount, string URL, string Username, string Password)
        {
            try
            {
                var balances = BundleAPIClient.AccountBalance(URL, Username, Password);
                var balance = balances.AccountBalances.First();

                if (result.Status == 1)
                {
                    result.InitialWalletBalance = (double)((balance.Amount + Amount) / 100);
                    result.FinalWalletBalance = (double)((decimal)balance.Amount / 100);
                }
                else
                {
                    result.InitialWalletBalance = (double)((decimal)balance.Amount / 100);
                }

            }
            catch (Exception)
            {
            }
        }


        [WebMethod]
        public AccountBalanceResponse GetBalance(string AppKey, string Currency = "")
        {
            if (AppKey != EconetAppKey) return new AccountBalanceResponse
            {
                RawResponseData = "Login Failed with AppKey"
            };

            try
            {
                var client = new Client(BasePrepaidURL);
                if (Currency == "1")
                {
                    return client.AccountBalanceUsdAsync().Result;
                }
                else
                {
                    return client.AccountBalanceZwlAsync().Result;
                }
            }
            catch (Exception ex)
            {
                return new AccountBalanceResponse
                {
                    RawResponseData = ex.Message
                };
            }
        }
        //public AccountBalanceResponse GetBalance(string AppKey, string Currency = "")
        //{
        //    if (AppKey != EconetAppKey) return new AccountBalanceResponse
        //    {
        //        RawResponseData = "Login Failed with AppKey"
        //    };

        //    try
        //    {
        //        if (Currency == "1")
        //        {
        //            return BundleAPIClient.AccountBalance(BundleAPIClient.URL, USDUsername, USDPassword);
        //        }
        //        else
        //        {
        //            return BundleAPIClient.AccountBalanceV2(BundleAPIClient.URL, ZWLUsername, ZWLPassword);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new AccountBalanceResponse
        //        {
        //            RawResponseData = ex.Message
        //        };
        //    }
        //}

        [WebMethod]
        public List<Hot.EconetBundle.Models.BundleProduct> GetBundles(string AppKey)
        {
            ReturnObject iRet = new ReturnObject();
            if (AppKey != EconetAppKey) return new List<Hot.EconetBundle.Models.BundleProduct>();
            if (bundles.Count() == 0) LoadBundles();
            return bundles;
        }


        private Hot.EconetBundle.Models.BundleProduct GetBundle(string BundleCode)
        {
            var testbundles = GetTestBundles();

            if (bundles.Count() == 0) LoadBundles();
            if (bundleList.ContainsKey(BundleCode)) return bundleList[BundleCode];
            if (testbundles.ContainsKey(BundleCode)) return testbundles[BundleCode];
            throw new Exception("Invalid Bundle - " + BundleCode + " is not a valid bundle product code.");
        }

        private Dictionary<string, Hot.EconetBundle.Models.BundleProduct> GetTestBundles()
        {
            return new Dictionary<string, Hot.EconetBundle.Models.BundleProduct>()
            {
                // { "ECR1",new BundleProduct(){ ProductCode = "ECR1" , Amount = 100 } },
            };
        }
        private void LoadBundles()
        {
            using (SqlConnection sqlConnection = new SqlConnection(_ConnectionString))
            {
                sqlConnection.Open();
                bundles = BundleRepository.List(sqlConnection);
                bundleList.Clear();
                foreach (Hot.EconetBundle.Models.BundleProduct i in bundles)
                {
                    bundleList.Add(i.ProductCode, i);
                }
                sqlConnection.Close();
            }
        }

    }

}
