using Horizon.XmlRpc.Client;
using Horizon.XmlRpc.Core;
using Hot.Econet.Prepaid.Enums;
using Hot.Econet.Prepaid.Interfaces.APIEndpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hot.Econet.Prepaid.Services;


public class ApiClient
{
    static public String URL = "https://emr-econet-zw.patternmatched.com:7002/xmlrpc";

    static public String USERNAME = "CommShopXMLUser";
    static public String PASSWORD = "CommShopXMLUserPMT52";
    static private readonly String PROVIDERCODE = Convert.ToString((int)ProviderCodes.Econet_ZIM);
    static public int CURRENCYCODE = (int)CurrencyCode.RTGS;

    #region "   Common   "
    public ApiClient(String Username = "", String Password = "", String Url = "")
    {
        USERNAME = (Username == "" ? USERNAME : Username);
        PASSWORD = (Password == "" ? PASSWORD : Password);
        URL = (Url == "" ? URL : Url);
        ConfiguredTLS();
    }

    void ConfiguredTLS()
    {
        try
        { //try TLS 1.3
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)12288
                                                 | (SecurityProtocolType)3072
                                                 | (SecurityProtocolType)768
                                                 | SecurityProtocolType.Tls;
        }
        catch (NotSupportedException)
        {
            throw;
        }
    }
    #endregion

    #region  "   Load Bundle Functions   "

    public LoadDataResponse LoadDataBundle(String targetMobile, int Amount, String ProductCode, String Reference, CurrencyCode currencyCode)
    {
        try
        {

            var proxy = XmlRpcProxyGen.Create<IEconetServiceProxy>();
            proxy.Url = URL;

            var bundleRequest = new LoadDataRequest()
            {
                MSISDN = targetMobile,
                ProviderCode = PROVIDERCODE,
                AccountType = 0,
                Currency = (int)currencyCode,
                Amount = Amount,
                ProductCode = ProductCode,
                Reference = Reference,
                Quantity = 1
            };

            try
            {
                var bundleResponse = proxy.purchase_bundle(USERNAME, PASSWORD, bundleRequest);
                return LoadDataResponse.Get(bundleResponse);
            }
            catch (XmlRpcException serverException)
            {
                throw new Exception("Fault " + serverException.Message + ": " + serverException.InnerException?.Message);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    public static LoadDataResponse LoadDataBundle(String targetMobile, int Amount, String ProductCode, String Reference, String Url = "", String Username = "", String Password = "", CurrencyCode currencyCode = CurrencyCode.RTGS)
    {
        var client = new ApiClient(Username, Password, Url);
        CURRENCYCODE = (int)currencyCode;
        return client.LoadDataBundle(targetMobile, Amount, ProductCode, Reference, currencyCode);
    }



    #endregion

    #region  "   Load Airtime Functions   "

    public LoadAirtimeResponse LoadAirtime(String targetMobile, int Amount, String Reference, CurrencyCode currencyCode)
    {
        try
        {
            var Request = new LoadAirtimeRequest()
            {
                MSISDN = targetMobile,
                ProviderCode = PROVIDERCODE,
                AccountType = 0,
                Currency = (int)currencyCode,
                Amount = Amount,
                Reference = Reference,
            };
            try
            {
                var proxy = XmlRpcProxyGen.Create<IEconetServiceProxy>();
                proxy.Url = URL;
                var response = proxy.purchase_airtime(USERNAME, PASSWORD, Request);
                return LoadAirtimeResponse.Get(response);
            }
            catch (XmlRpcException serverException)
            {
                throw new Exception("Fault " + serverException.Message + ": " + serverException.InnerException?.Message);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public static LoadAirtimeResponse LoadAirtime(String targetMobile, int Amount, String Reference, String Url = "", String Username = "", String Password = "", CurrencyCode currencyCode = CurrencyCode.RTGS)
    {
        var client = new ApiClient(Username, Password, Url);
        CURRENCYCODE = (int)currencyCode;
        return client.LoadAirtime(targetMobile, Amount, Reference, currencyCode);
    }

    #endregion

    #region "   Account Balance Enquiry   "

    public AccountBalanceResponse AccountBalance()
    {
        try
        {
            try
            {
                var proxy = XmlRpcProxyGen.Create<IEconetServiceProxy>();
                proxy.Url = URL;
                var response = proxy.account_balance_enquiry(USERNAME, PASSWORD);
                return AccountBalanceResponse.Get(response);
            }
            catch (XmlRpcException serverException)
            {
                throw new Exception("Fault " + serverException.Message + ": " + serverException.InnerException?.Message);
            }
        }
        catch (Exception )
        {
            throw;
        }
    }
    public static AccountBalanceResponse AccountBalance(String Url = "", String Username = "", String Password = "")
    {
        ApiClient myClass = new ApiClient(Username, Password, Url);
        return myClass.AccountBalance();
    }

    public static AccountBalanceResponse AccountBalanceV2(string Url = "", string Username = "", string Password = "")
    {
        var result = new AccountBalanceResponse() { AccountBalances = new List<AccountBalance>() };
        using (var client = new HttpClient())
        {
            var request = $"<?xml version=\"1.0\" encoding=\"us-ascii\"?><methodCall><methodName>account_balance_enquiry</methodName><params><param><value><string>{Username}</string></value></param><param><value><string>{Password}</string></value></param></params></methodCall>";
            var stringContent = new StringContent(request, Encoding.UTF8, "text/xml");

            using (var response = client.PostAsync(URL, stringContent).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    result = ManuallyParseData(data);
                }
            }
        }
        return result;
    }

    private static AccountBalanceResponse ManuallyParseData(string data)
    {
        var serializer = new XmlSerializer(typeof(methodResponse));
        var memStream = new MemoryStream(Encoding.UTF8.GetBytes(data));
        methodResponse parsedObject = (methodResponse)(serializer.Deserialize(memStream) ?? new());

        var result = new AccountBalanceResponse() { AccountBalances = new List<AccountBalance>(), RawResponseData = data };
        var list = parsedObject?.@params.param.value.array.data;
        if (list is null) return new();
        foreach (var item in list)
        {
            var fields = item.@struct;
            AccountBalance accountBalance = new AccountBalance()
            {
                AccountType = (int)fields[0].value.@int,
                Currency = (int)fields[1].value.@int,
                Amount = (long)fields[2].value.@int
            };
            result.AccountBalances.Add(accountBalance);
        }

        return result;
    }
    #endregion

    #region "   Get Bundles Enquiry   "

    //public BundleProductResponse GetBundleProducts()
    //{
    //    try
    //    {
    //        XmlRpcRequest Request = GetBundleProductsRequest();
    //        try
    //        {
    //            return BundleProductResponse.Get(Request.Send(URL));
    //        }
    //        catch (XmlRpcException serverException)
    //        {
    //            throw new Exception("Fault " + serverException.FaultCode + ": " + serverException.FaultString);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //public static BundleProductResponse GetBundleProducts(String Url = "", String Username = "", String Password = "")
    //{
    //    BundleAPIClient myClass = new BundleAPIClient(Username, Password, Url);
    //    return myClass.GetBundleProducts();
    //}

    #endregion

    #region  "   Get Transaction Status Functions   "

    //private XmlRpcRequest GetTransactionStatusRequest(GetTransactionStatusRequest request)
    //{
    //    XmlRpcRequest rpcRequest = RequestBuilder(BundleAPIMethods.GET_TRANSACTION_STATUS_METHOD);
    //    rpcRequest.Params.Add(request);
    //    return rpcRequest;
    //}

    //public GetTransactionStatusResponse GetTransactionStatus(String targetMobile, String Reference)
    //{
    //    try
    //    {
    //        XmlRpcRequest Request = GetTransactionStatusRequest(new GetTransactionStatusRequest()
    //        {
    //            MSISDN = targetMobile,
    //            Reference = Reference
    //        });
    //        try
    //        {
    //            return GetTransactionStatusResponse.Get(Request.Send(URL));
    //        }
    //        catch (XmlRpcException serverException)
    //        {
    //            throw new Exception("Fault " + serverException.FaultCode + ": " + serverException.FaultString);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public static GetTransactionStatusResponse GetTransactionStatus(String targetMobile, String Reference, String Url = "", String Username = "", String Password = "")
    //{
    //    BundleAPIClient myClass = new BundleAPIClient(Username, Password, Url);
    //    return myClass.GetTransactionStatus(targetMobile, Reference);
    //}

    #endregion

}