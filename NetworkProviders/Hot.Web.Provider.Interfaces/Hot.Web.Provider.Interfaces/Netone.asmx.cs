using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Hot.API.Interface;
using Hot.API.Interface.Models;
using System.Configuration;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Hot.API.Interface.Extensions;
using System.Data.SqlClient;
using Hot.Web.Provider.Interfaces.Services;

namespace Hot.Web.Provider.Interfaces
{
    /// <summary>
    /// Summary description for Netone
    /// </summary>
    [WebService(Namespace = "http://hot.co.zw:8007/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Netone : WebService
    {
        static readonly string _ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
        public static string EconetAppKey = ConfigurationManager.AppSettings["EconetAppKey"];
        static readonly string baseUrl = ConfigurationManager.AppSettings["NetoneAPIBaseUrl"];
        static readonly string AccessCode = ConfigurationManager.AppSettings["NetoneAPIAccessCode"];
        static readonly string AccessPassword = ConfigurationManager.AppSettings["NetoneAPIAccessPassword"];
        static readonly bool NetoneAPIDisabled = Convert.ToBoolean(ConfigurationManager.AppSettings["NetoneAPIDisabled"]);
        static readonly bool NetoneAPIZWLDisabled = Convert.ToBoolean(ConfigurationManager.AppSettings["NetoneAPIZWLDisabled"]);
        // static readonly bool DisableCertErrors = Convert.ToBoolean(ConfigurationManager.AppSettings["NetoneAPISSLCheckDisabled"]);
        private HotAPIClient NetoneAPIClient = new HotAPIClient(baseUrl, AccessCode, AccessPassword, Convert.ToBoolean(ConfigurationManager.AppSettings["NetoneAPISSLCheckDisabled"]));
        private List<Hot.EconetBundle.Models.BundleProduct> bundles = new List<Hot.EconetBundle.Models.BundleProduct>();
        private Dictionary<string, Hot.EconetBundle.Models.BundleProduct> bundleList = new Dictionary<string, Hot.EconetBundle.Models.BundleProduct>();


        [WebMethod]
        public RechargeResponse RechargeBundle(string AppKey, string TargetMobile, string ProductCode, string Amount, long RechargeID)
        {
            if (AppKey != EconetAppKey) return
                   new RechargeResponse
                   {
                       ReplyMessage = "Login Failed with AppKey",
                       ReplyCode = (int)ReplyCode.FailedWebLogin
                   };
            if (NetoneAPIDisabled) return
                    new RechargeResponse
                    {
                        ReplyMessage = "Provider Error Code: 36188",
                        ReplyMsg = "Provider Error Code: 36188",
                        ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform
                    };

            try
            {
                RechargeResponse response = NetoneAPIClient.RechargeDataAsync(
                        TargetMobile,
                        ProductCode,
                        "Hot-" + RechargeID.ToString(),
                        Convert.ToDecimal(Amount)
                   ).Result;

                if (response.ReplyMsg.ToLower().Contains("task was canceled")) throw new Exception(response.ReplyMsg);

                if (response.ReplyCode == (int)ReplyCode.FailedInsufficeintWalletBalance || response.ReplyMsg.ToLower().Contains("insufficient"))
                {
                    response.ReplyMessage = "Provider Error Code: 36180";
                    response.ReplyMsg = "Provider Error Code: 36180";
                    response.ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform;
                }
                if (response.ReplyMsg.Contains("Failed To Recharge Error# Object reference not set to an"))
                {
                    response.ReplyMessage = "Provider Error Code: 36170";
                    response.ReplyMsg = "Provider Error Code: 36170";
                    response.ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform;
                }
                return response;
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.ToLower().Contains("insufficient"))
                {
                    return new RechargeResponse()
                    {
                        ReplyMessage = "Provider Error Code: 36180",
                        ReplyMsg = "Provider Error Code: 36180",
                        ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform,
                    };

                }
                if (ex.InnerException.Message.Contains("timeout") || ex.InnerException.Message.Contains("task was canceled"))
                {
                    try
                    {
                        return QueryTransaction(RechargeID);
                    }
                    catch
                    {
                    }
                }
                return new RechargeResponse
                {
                    ReplyMessage = ex.InnerException.Message,
                    ReplyCode = (int)ReplyCode.FailedWebException
                };
            }
        }
        [WebMethod]
        public RechargeResponse RechargeUSDBundle(string AppKey, string TargetMobile, string ProductCode, string Amount, long RechargeID)
        {
            if (AppKey != EconetAppKey) return
                   new RechargeResponse
                   {
                       ReplyMessage = "Login Failed with AppKey",
                       ReplyCode = (int)ReplyCode.FailedWebLogin
                   };
            if (NetoneAPIDisabled) return
                    new RechargeResponse
                    {
                        ReplyMessage = "Provider Error Code: 36188",
                        ReplyMsg = "Provider Error Code: 36188",
                        ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform
                    };

            try
            {
                RechargeResponse response = NetoneAPIClient.RechargeDataUSDAsync(
                        TargetMobile,
                        ProductCode,
                        "Hot-" + RechargeID.ToString(),
                        Convert.ToDecimal(Amount)
                   ).Result;

                if (response.ReplyMsg.ToLower().Contains("task was canceled")) throw new Exception(response.ReplyMsg);

                if (response.ReplyCode == (int)ReplyCode.FailedInsufficeintWalletBalance || response.ReplyMsg.ToLower().Contains("insufficient"))
                {
                    response.ReplyMessage = "Provider Error Code: 36180";
                    response.ReplyMsg = "Provider Error Code: 36180";
                    response.ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform;
                }
                if (response.ReplyMsg.Contains("Failed To Recharge Error# Object reference not set to an"))
                {
                    response.ReplyMessage = "Provider Error Code: 36170";
                    response.ReplyMsg = "Provider Error Code: 36170";
                    response.ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform;
                }
                return response;
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.ToLower().Contains("insufficient"))
                {
                    return new RechargeResponse()
                    {
                        ReplyMessage = "Provider Error Code: 36180",
                        ReplyMsg = "Provider Error Code: 36180",
                        ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform,
                    };

                }
                if (ex.InnerException.Message.Contains("timeout") || ex.InnerException.Message.Contains("task was canceled"))
                {
                    try
                    {
                        return QueryTransaction(RechargeID);
                    }
                    catch
                    {
                    }
                }
                return new RechargeResponse
                {
                    ReplyMessage = ex.InnerException.Message,
                    ReplyCode = (int)ReplyCode.FailedWebException
                };
            }
        }


        [WebMethod]
        public RechargeResponse RechargeMobile(string AppKey, string TargetMobile, string Amount, long RechargeID)
        {
            if (AppKey != EconetAppKey) return
                    new RechargeResponse
                    {
                        ReplyMessage = "Login Failed with AppKey",
                        ReplyCode = (int)ReplyCode.FailedWebLogin
                    };
            if (NetoneAPIDisabled) return
                    new RechargeResponse
                    {
                        ReplyMessage = "Provider Error Code: 36188",
                        ReplyMsg = "Provider Error Code: 36188",
                        ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform
                    };
            if (NetoneAPIZWLDisabled) return
                 new RechargeResponse
                 {
                     ReplyMessage = "Provider Error Code: 36188",
                     ReplyMsg = "Provider Error Code: 36188",
                     ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform
                 };
            try
            {
                RechargeResponse response = NetoneAPIClient.RechargeAsync(
                        TargetMobile,
                        Convert.ToDecimal(Amount),
                        "Hot-" + RechargeID.ToString()
                   ).Result;

                if (response.ReplyMsg.ToLower().Contains("task was canceled")) throw new Exception(response.ReplyMsg);

                if (response.ReplyCode == (int)ReplyCode.FailedInsufficeintWalletBalance || response.ReplyMsg.ToLower().Contains("insufficient"))
                {
                    response.ReplyMessage = "Provider Error Code: 36180";
                    response.ReplyMsg = "Provider Error Code: 36180";
                    response.ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform;
                }
                if (response.ReplyMsg.Contains("Failed To Recharge Error# Object reference not set to an"))
                {
                    response.ReplyMessage = "Provider Error Code: 36170";
                    response.ReplyMsg = "Provider Error Code: 36170";
                    response.ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform;
                }
                return response;
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.ToLower().Contains("insufficient"))
                {
                    return new RechargeResponse()
                    {
                        ReplyMessage = "Provider Error Code: 36180",
                        ReplyMsg = "Provider Error Code: 36180",
                        ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform,
                    }; 
                }

                if (ex.InnerException.Message.Contains("timeout") || ex.InnerException.Message.Contains("task was canceled"))
                {
                    try
                    {
                        return QueryTransaction(RechargeID);
                    }
                    catch
                    {
                    }
                }
                return new RechargeResponse
                {
                    ReplyMessage = ex.InnerException.Message,
                    ReplyCode = (int)ReplyCode.FailedWebException
                };
            }
        }
        [WebMethod]
        public RechargeBundledResponse RechargeMobileUSD(string AppKey, string TargetMobile, string Amount, long RechargeID)
        {
            if (AppKey != EconetAppKey) return
                    new RechargeBundledResponse
                    {
                        ReplyMessage = "Login Failed with AppKey",
                        ReplyCode = (int)ReplyCode.FailedWebLogin
                    };
            if (NetoneAPIDisabled) return
                    new RechargeBundledResponse
                    {
                        ReplyMessage = "Provider Error Code: 36188",
                        ReplyMsg = "Provider Error Code: 36188",
                        ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform
                    };
           
            try
            {
                RechargeBundledResponse response = NetoneAPIClient.RechargeBundledAsync(
                        TargetMobile,
                        Convert.ToDecimal(Amount),
                        "Hot-" + RechargeID.ToString()
                   ).Result;

                if (response.ReplyMsg.ToLower().Contains("task was canceled")) throw new Exception(response.ReplyMsg);

                if (response.ReplyCode == (int)ReplyCode.FailedInsufficeintWalletBalance || response.ReplyMsg.ToLower().Contains("insufficient"))
                {
                    response.ReplyMessage = "Provider Error Code: 36180";
                    response.ReplyMsg = "Provider Error Code: 36180";
                    response.ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform;
                }
                if (response.ReplyMsg.Contains("Failed To Recharge Error# Object reference not set to an"))
                {
                    response.ReplyMessage = "Provider Error Code: 36170";
                    response.ReplyMsg = "Provider Error Code: 36170";
                    response.ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform;
                }
                return response;
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.ToLower().Contains("insufficient"))
                {
                    return new RechargeBundledResponse()
                    {
                        ReplyMessage = "Provider Error Code: 36180",
                        ReplyMsg = "Provider Error Code: 36180",
                        ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform,
                    };
                }

                if (ex.InnerException.Message.Contains("timeout") || ex.InnerException.Message.Contains("task was canceled"))
                {
                    try
                    {
                        return (RechargeBundledResponse)QueryTransaction(RechargeID);
                    }
                    catch
                    {
                    }
                }
                return new RechargeBundledResponse
                {
                    ReplyMessage = ex.InnerException.Message,
                    ReplyCode = (int)ReplyCode.FailedWebException
                };
            }
        }

        private RechargeResponse QueryTransaction(long RechargeID)
        {
            var result = NetoneAPIClient.QueryTransaction("Hot-" + RechargeID.ToString());
            var response = JsonConvert.DeserializeObject<RechargeResponse>(result.RawReply);
            return response;
        }

        [WebMethod]
        public EndUserBalanceResponse UserBalance(string AppKey, string TargetMobile)
        {
            if (AppKey != EconetAppKey) return
                   new EndUserBalanceResponse
                   {
                       ReplyMsg = "Login Failed with AppKey",
                       ReplyCode = (int)ReplyCode.FailedWebLogin
                   };

            try
            {
                return NetoneAPIClient.GetEndUserBalanceAsync(
                       TargetMobile
                  ).Result;
            }
            catch (Exception ex)
            {
                return new EndUserBalanceResponse
                {
                    ReplyMsg = ex.InnerException.Message,
                    ReplyCode = (int)ReplyCode.FailedWebException
                };
            }
        }

        [WebMethod]
        public WalletBalanceResponse GetBalance(string AppKey)
        {
            if (AppKey != EconetAppKey) return new WalletBalanceResponse()
            {
                ReplyCode = (int)ReplyCode.FailedWebLogin,
                ReplyMsg = "Login Failed with AppKey"
            };


            try
            {
                return NetoneAPIClient.GetWalletBalanceAsync().Result;
            }
            catch (Exception ex)
            {
                return new WalletBalanceResponse
                {
                    ReplyMsg = ex.InnerException.Message,
                    ReplyCode = (int)ReplyCode.FailedWebException
                };
            }
        }

        [WebMethod]
        public WalletBalanceResponse GetUSDBalance(string AppKey)
        {
            if (AppKey != EconetAppKey) return new WalletBalanceResponse()
            {
                ReplyCode = (int)ReplyCode.FailedWebLogin,
                ReplyMsg = "Login Failed with AppKey"
            };


            try
            {
                return NetoneAPIClient.GetWalletBalanceUSDAsync().Result;
            }
            catch (Exception ex)
            {
                return new WalletBalanceResponse
                {
                    ReplyMsg = ex.InnerException.Message,
                    ReplyCode = (int)ReplyCode.FailedWebException
                };
            }
        }


        [WebMethod]
        public RechargeResponse QueryRecharge(string AppKey, string AgentReference)
        {
            try
            {
                var result = NetoneAPIClient.QueryTransaction(AgentReference);
                var response = JsonConvert.DeserializeObject<RechargeResponse>(result.RawReply);
                return response;
            }
            catch (Exception ex)
            {
                return new RechargeResponse
                {
                    ReplyMessage = ex.InnerException.Message,
                    ReplyCode = (int)ReplyCode.FailedWebException
                };
            }
        }

        [WebMethod]
        public BulkEvdResponse BulkPinSale(string AppKey, int BrandId, decimal Denomination, int Quantity, long RechargeID)
        {
            if (AppKey != EconetAppKey) return
                   new BulkEvdResponse
                   {
                       ReplyMsg = "Login Failed with AppKey",
                       ReplyCode = (int)ReplyCode.FailedWebLogin
                   };
            if (NetoneAPIDisabled) return
                   new BulkEvdResponse
                   {
                       ReplyMsg = "Provider Error Code: 36188",
                       ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform
                   };
            if (NetoneAPIZWLDisabled) return
                  new BulkEvdResponse
                  {
                      ReplyMsg = "Provider Error Code: 36188",
                      ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform
                  };

            try
            {
                BulkEvdResponse response = NetoneAPIClient.BulkEvdSaleAsync(
                        BrandId,
                        Denomination,
                        Quantity,
                        "Hot-" + RechargeID.ToString()
                   ).Result;

                if (response.ReplyMsg.ToLower().Contains("task was canceled")) throw new Exception(response.ReplyMsg);

                if (response.ReplyCode == (int)ReplyCode.FailedInsufficeintWalletBalance || response.ReplyMsg.ToLower().Contains("insufficient"))
                {
                    response.ReplyMsg = "Provider Error Code: 36180";
                    response.ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform;
                }
                if (response.ReplyMsg.Contains("Object reference not set to an"))
                {
                    response.ReplyMsg = "Provider Error Code: 36170";
                    response.ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform;
                }
                return response;
            }
            catch (Exception ex)
            {

                return new BulkEvdResponse
                {
                    ReplyMsg = ex.InnerException.Message,
                    ReplyCode = (int)ReplyCode.FailedWebException
                };
            }
        }

        [WebMethod]
        public QueryEvdResponse QueryEvdStock(string AppKey)
        {
            if (AppKey != EconetAppKey) return
                   new QueryEvdResponse
                   {
                       ReplyMsg = "Login Failed with AppKey",
                       ReplyCode = (int)ReplyCode.FailedWebLogin
                   };
            if (NetoneAPIDisabled) return
                   new QueryEvdResponse
                   {
                       ReplyMsg = "Provider Error Code: 36188",
                       ReplyCode = (int)ReplyCode.FailedNetworkPrepaidPlatform
                   };

            try
            {
                QueryEvdResponse response = NetoneAPIClient.QueryEvdStockAsync().Result;

                if (response.ReplyMsg.ToLower().Contains("task was canceled")) throw new Exception(response.ReplyMsg);

                return response;
            }
            catch (Exception ex)
            {

                return new QueryEvdResponse
                {
                    ReplyMsg = ex.InnerException.Message,
                    ReplyCode = (int)ReplyCode.FailedWebException
                };
            }
        }

        [WebMethod]
        public BulkEvdResponse QueryEvdTransaction(string AppKey, long RechargeId)
        {
            if (AppKey != EconetAppKey) return
                   new BulkEvdResponse
                   {
                       ReplyMsg = "Login Failed with AppKey",
                       ReplyCode = (int)ReplyCode.FailedWebLogin
                   };

            try
            {
                BulkEvdResponse response = NetoneAPIClient.QueryEvdSaleAsync($"{RechargeId}").Result;

                if (response.ReplyMsg.ToLower().Contains("task was canceled")) throw new Exception(response.ReplyMsg);

                return response;
            }
            catch (Exception ex)
            {

                return new BulkEvdResponse
                {
                    ReplyMsg = ex.InnerException.Message,
                    ReplyCode = (int)ReplyCode.FailedWebException
                };
            }
        }

        [WebMethod]
        public List<Hot.EconetBundle.Models.BundleProduct> GetBundles(string AppKey)
        {
            ReturnObject iRet = new ReturnObject();
            if (AppKey != EconetAppKey) return new List<Hot.EconetBundle.Models.BundleProduct>();
            if (bundles.Count() == 0) LoadBundles();
            return bundles;
        }

        private void LoadBundles()
        {
            var netoneBrandIds = new List<int>() { 38, 39, 42, 26, 25, 40 };
            using (SqlConnection sqlConnection = new SqlConnection(_ConnectionString))
            {

                sqlConnection.Open();
                bundles = BundleRepository.List(sqlConnection)
                    .Where(i => netoneBrandIds.Any(x => x == i.BrandId))
                    .ToList();
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

