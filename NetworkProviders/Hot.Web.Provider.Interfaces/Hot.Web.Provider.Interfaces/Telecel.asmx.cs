using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using eJuice;
using Hot.Web.Provider.Interfaces.eJuiceService;

namespace Hot.Web.Provider.Interfaces
{
    /// <summary>
    /// Summary description for Telecel
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]

    public class Telecel : System.Web.Services.WebService
    {
        static readonly bool TelecelAPIDisabled = Convert.ToBoolean(ConfigurationManager.AppSettings["TelecelAPIDisabled"]);
        static readonly string TelecelAPIURL = ConfigurationManager.AppSettings["TelecelAPIURL"];


        [WebMethod]
        public TopupResponse JuiceRecharge(string AgentCode, string MPin, string TargetMobile, string Amount, string TransID)
        {
            TopupResponse response = new TopupResponse();
            try
            {
                if (TelecelAPIDisabled) return
                    new TopupResponse
                    {
                        resultcode = "-1",
                        responseValue = "Provider Error Code: 36188"
                    };

                Thread.Sleep((new Random()).Next(330, 1589)); // Random sleep to fix Telecel Concurrency issue
                response = (new EstelTprServicesClient()).getTopup(
                        new TopupRequest()
                        {
                            agentCode = AgentCode.TrimStart('0'),
                            mpin = MPin,
                            destination = TargetMobile.TrimStart('0'),
                            amount = Amount,
                            agenttransid = TransID
                        }
                    );
                if (response.resultdescription == "Insufficient Wallet")
                {
                    response.resultdescription = "Provider Error Code: 36180";
                }
                if (response.resultdescription.StartsWith("Template Not Found"))
                {
                    response.resultdescription = "Recharge below Minimum of $1";
                }
            }
            catch (Exception)
            {
                response.resultcode = "-1";
                response.responseValue = "Provider Webservice Failure: Telecel webservice Failed";
            }
            return response;
        }

        [WebMethod]
        public TopupResponse JuiceRechargeUSD(string AgentCode, string MPin, string TargetMobile, string Amount, string TransID)
        {
            TopupResponse response = new TopupResponse();
            try
            {
                if (TelecelAPIDisabled) return
                    new TopupResponse
                    {
                        resultcode = "-1",
                        responseValue = "Provider Error Code: 36188"
                    };

                Thread.Sleep((new Random()).Next(330, 1589)); // Random sleep to fix Telecel Concurrency issue
                response = (new EstelTprServicesClient()).getTopup(
                        new TopupRequest()
                        {
                            agentCode = AgentCode.TrimStart('0'),
                            mpin = MPin,
                            destination = TargetMobile.TrimStart('0'),
                            amount = Amount,
                            agenttransid = TransID,
                            productCode = "USD_ON_US"
                        }
                    );
                if (response.resultdescription == "Insufficient Wallet")
                {
                    response.resultdescription = "Provider Error Code: 36180";
                }
                if (response.resultdescription.StartsWith("Template Not Found"))
                {
                    response.resultdescription = "Recharge below Minimum of $1";
                }
            }
            catch (Exception)
            {
                response.resultcode = "-1";
                response.responseValue = "Provider Webservice Failure: Telecel webservice Failed";
            }
            return response;
        }


        [WebMethod]
        public BalanceResponse JuiceBalance(string AgentCode, string MPin)
        {
            BalanceResponse response = new BalanceResponse();
            try
            {
                 
                //EstelTprServicesClient client = new EstelTprServicesClient();
                 response = (new EstelTprServicesClient()).getBalance(new BalanceRequest() { agentCode = AgentCode.TrimStart('0'), mpin = MPin });
                //var client = new TelecelClient(TelecelAPIURL);
                //  response = client.GetBalance( AgentCode.TrimStart('0'),MPin );
                var balances = GetBalances(response.amount).Where(b => b.Currency == "ZWL").ToList();
                if (balances.Count > 0) response.amount = balances.First().Amount.ToString();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                response.resultcode = "-1";
                response.responsevalue = "Provider Webservice Failure: Telecel webservice Failed";
            }
            return response;
        }

        [WebMethod]
        public BalanceResponse JuiceBalanceUSD(string AgentCode, string MPin)
        {
            BalanceResponse response = new BalanceResponse();
            try
            {
                //response = (new EstelTprServicesClient()).getBalance(
                //        new BalanceRequest()
                //        {
                //            agentCode = AgentCode.TrimStart('0'),
                //            mpin = MPin
                //        }
                //    );
                var client = new TelecelClient(TelecelAPIURL);
                response = client.GetBalance(AgentCode.TrimStart('0'), MPin);
                var balances = GetBalances(response.amount).Where(b => b.Currency == "USD").ToList();
                if (balances.Count > 0) response.amount = balances.First().Amount.ToString();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                response.resultcode = "-1";
                response.responsevalue = "Provider Webservice Failure: Telecel webservice Failed";
            }
            return response;
        }


        List<Balance> GetBalances(string data)
        {
            var list = new List<Balance>();
            if (!((data.Length % 20) == 0)) return list;
            for (int i = 0; i < data.Length; i += 20)
            {
                var balance = ParseBalance(data.Substring(i, 20));
                if (!(balance is null)) list.Add(balance);
            }
            return list;
        }

        Balance ParseBalance(string data)
        {
            if (data.Length != 20) return null;
            var CurrencyCode = data.Substring(4, 3);
            var AmountFormat = data.Substring(7, 1);
            var amountinCents = data.Substring(8, 12);
            var isvalid = int.TryParse(amountinCents, out int Value);
            if (!isvalid) return null;
            Value = AmountFormat == "C" ? Value : -Value;
            CurrencyCode = CurrencyCode == "932" ? "ZWL" : "USD";
            return new Balance(Value, CurrencyCode);
        }


    }

    public class Balance
    {
        public Balance(int amount, string currency)
        {
            Value = amount;
            Currency = currency;
        }

        public int Value { get; set; }
        public string Currency { get; set; }

        public decimal Amount { get { return (decimal)Value / (decimal)100.00; } }

    }

    public class TelecelClient
    {
        public TelecelClient(string url)
        {
            Url = url;
        }

        public string Url { get; set; }
        public BalanceResponse GetBalance(string agentCode, string mpin)
        {
            HttpWebRequest request = CreateSOAPWebRequest(Url);

            XmlDocument SOAPReqBody = new XmlDocument();
            //SOAP Body Request
            SOAPReqBody.LoadXml(@"<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">
        <s:Body>
            <getBalance xmlns=""http://services.estel.com""
                xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"">
                <balanceRequest xmlns:a=""http://balance.support.services.estel.com"">
                    <a:agentCode>" + agentCode + @"</a:agentCode>
                    <a:mpin>" + mpin + @"</a:mpin>
                </balanceRequest>
            </getBalance>
        </s:Body>
    </s:Envelope>");

            using (Stream stream = request.GetRequestStream())
            {
                SOAPReqBody.Save(stream);
            }
            //Geting response from request
            using (WebResponse Serviceres = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
                {
                    //reading stream
                    var ServiceResult = rd.ReadToEnd();
                    //writting stream result on console
                    try
                    {
                        //var start = ServiceResult.IndexOf("<getBalanceReturn");
                        //var end = ServiceResult.IndexOf("</getBalanceReturn");
                        //var balResponse = ServiceResult.Substring(start, end - start)
                        //    .Replace("xsi:type=\"xsd:string\"", "")
                        //    .Replace("xsi:nil=\"true\"", "")
                        //    .Replace("xsi:type=\"ns2:BalanceResponse\"",""); 
                        var response = DeserializeToObject<Envelope>(ServiceResult);
                        var balresponse = response.Body.getBalanceResponse.getBalanceReturn;
                        var result = new BalanceResponse()
                        {
                            agentcode = $"{balresponse.agentcode}",
                            agentname = $"{balresponse.agentname}",
                            agenttransid = $"{balresponse.agenttransid}",
                            amount = $"{balresponse.amount}",
                            clienttype = $"{balresponse.clienttype}",
                            comments = $"{balresponse.comments}",
                            destination = $"{balresponse.destination}",
                            requestcts = balresponse.requestcts,
                            responsects = balresponse.responsects,
                            responsevalue = $"{balresponse.responsevalue}",
                            resultcode = $"{balresponse.resultcode}",
                            resultdescription = balresponse.resultdescription,
                            source = $"{balresponse.source}",
                            transid = $"{balresponse.transid}",
                            vendorcode = balresponse.vendorcode
                        };
                        return result;
                    }
                    catch (Exception)
                    {
                        return new BalanceResponse();
                    }
                }
            }
        }

        public TopupResponse Topup(TopupRequest topupRequest)
        {
            return new TopupResponse();
        }

        private HttpWebRequest CreateSOAPWebRequest(string url)
        {
            //Making Web Request
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(url);
            //SOAPAction
            Req.Headers.Add(@"SOAPAction:");
            //Content_type
            Req.ContentType = "text/xml;charset=\"utf-8\"";
            Req.Accept = "text/xml";
            //HTTP method
            Req.Method = "POST";
            //return HttpWebRequest
            return Req;
        }

        public static T DeserializeToObject<T>(string data) where T : class
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));

            using (TextReader reader = new StringReader(data))
            {
                return (T)ser.Deserialize(reader);
            }
        }
    }



}


