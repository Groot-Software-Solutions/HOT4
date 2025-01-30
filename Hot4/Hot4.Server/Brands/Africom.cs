//using Hot4.Core.Enums;
//using Hot4.DataModel.Models;
//using Hot4.Server.Brands;
//using Hot4.Service.Abstract;
//using Hot4.ViewModel;
//using Microsoft.Data.SqlClient;
//using Microsoft.Identity.Client;
//using System;
//using System.Reflection;

//public class Africom : NetworkBase
//{
//    private const string SuccessCode = "000";

//    //public Africom(SqlConnection sqlConn, string applicationName, string serviceEndpoint, bool isTestMode, string referencePrefix, bool webServiceOrCorporate)
//    //    : base(sqlConn, applicationName, serviceEndpoint, isTestMode, webServiceOrCorporate, referencePrefix)
//    //{
//    //}
//    private readonly IRechargePrepaidService _rechargePrepaidService;
//    public Africom(IRechargePrepaidService rechargePrepaidService)
//    {
//        _rechargePrepaidService = rechargePrepaidService;
//    }
//    public override ServiceRechargeResponse RechargePrepaid(RechargeModel recharge, int numberOfDays = 90)
//    {
//        ApplyDiscountRules(recharge);
//        var iRechargePrepaid = CreateRechargeObject(recharge);
//        try
//        {
//            iRechargePrepaid.DebitCredit = recharge.Amount >= 0;

//            if (!iRechargePrepaid.DebitCredit)
//            {
//                iRechargePrepaid.Narrative = "Africom Debit not possible";
//                iRechargePrepaid.ReturnCode = "099";
//                recharge.StateId = SmsState.Failure
//                return new ServiceRechargeResponse(iRechargePrepaid);
//            }
//            _rechargePrepaidService.AddRechargePrepaid(iRechargePrepaid);

//            var client = GetNetworkClient(40000);
//            try
//            {
//                try
//                {
//                    var africom = client.GetNetwork();
//                    Transfer response = null;
//                    try
//                    {
//                        var encryptResponse = GetEncryptResponse(recharge, iRechargePrepaid, africom);

//                        var transferRequest = new AfriTransferRequest
//                        {
//                            Body = new AfriTransferRequestBody
//                            {
//                                requestParam = "8644003060|" + encryptResponse
//                            }
//                        };
//                        response = africom.AfriTransfer(transferRequest).Body.AfriTransferResult;
//                        xLog_Data.WriteJsonToConsole(response);
//                    }
//                    catch (Exception ex)
//                    {
//                        recharge.State.StateID = xState.States.Failure;
//                        iRechargePrepaid.Narrative += "Exception: AfricomWebservice: " + ex;
//                        Log(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", recharge.RechargeID);
//                    }
//                    AssignRechargeFromResponse(recharge, iRechargePrepaid, response);
//                }
//                catch (Exception ex)
//                {
//                    Log(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", recharge.RechargeID);
//                }
//            }
//            catch (Exception ex)
//            {
//                // Just set StateID to a failure
//                recharge.State.StateID = xState.States.Failure;
//                iRechargePrepaid.Narrative = "Exception: " + ex;
//                Log(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", recharge.RechargeID);
//            }
//            recharge.RechargeDate = DateTime.Now;
//        }
//        catch (Exception ex)
//        {
//            recharge.State.StateID = xState.States.Failure;
//            iRechargePrepaid.Narrative = "Exception: " + ex;
//            Log(MethodBase.GetCurrentMethod().Name, ex, "RechargeID", recharge.RechargeID);
//        }

//        xRechargePrepaidAdapter.Save(iRechargePrepaid, SqlConn);
//        xRechargeAdapter.Save(recharge, SqlConn);
//        return new ServiceRechargeResponse(iRechargePrepaid);
//    }

//    private string GetEncryptResponse(xRecharge recharge, xRechargePrepaid iRechargePrepaid, ServiceSoap africom)
//    {
//        var encryptString = "8644003060|cD&7Q3|" + iRechargePrepaid.Reference +
//                            "|8644003060|prepaid_credit|" + recharge.Amount + "|" +
//                            recharge.Mobile.Substring(1);

//        var encryptRequest = new AfriEncryptRequest
//        {
//            Body = new AfriEncryptRequestBody
//            {
//                str = encryptString,
//                secretkey = "629V25eJ"
//            }
//        };
//        return africom.AfriEncrypt(encryptRequest).Body.AfriEncryptResult;
//    }

//    private void AssignRechargeFromResponse(xRecharge recharge, xRechargePrepaid iRechargePrepaid, Transfer response)
//    {
//        iRechargePrepaid.ReturnCode = response.THeader.Code;
//        iRechargePrepaid.Narrative = response.THeader.Description;

//        if (response.THeader.Code == SuccessCode) // Success
//        {
//            recharge.State.StateID = xState.States.Success;
//            iRechargePrepaid.InitialBalance = response.TransferType.TransfereeNoBalanceOld;
//            iRechargePrepaid.FinalBalance = response.TransferType.TransfereeNoBalanceNew;
//            iRechargePrepaid.Narrative = "Description:" + response.THeader.Description +
//                                         " |System:" + response.THeader.SystemReferenceNo +
//                                         " |ProductName:" + response.TransferType.ProductName +
//                                         " |Old Wallet:" +
//                                         response.TransferType.SubscriberBalanceOld +
//                                         " |New Wallet:" +
//                                         response.TransferType.SubscriberBalanceNew;
//            iRechargePrepaid.InitialWallet = response.TransferType.SubscriberBalanceOld;
//            iRechargePrepaid.FinalWallet = response.TransferType.SubscriberBalanceNew;
//        }
//        else // Failure
//        {
//            recharge.State.StateID = xState.States.Failure;
//            iRechargePrepaid.FinalBalance = -1;
//            iRechargePrepaid.InitialBalance = -1;
//        }
//    }

//    private void Log(string methodName, Exception ex, string idType, long idNumber)
//    {
//        xLog_Data.Save(ApplicationName, GetType().Name, methodName, ex, SqlConn.ConnectionString, idType, idNumber);
//    }

//    public override INetworkClient<ServiceSoap> GetNetworkClient(int timeout = 30000)
//    {
//        if (IsTestMode)
//        {
//            return GetTestClient();
//        }
//        return NetworkClientFactory.Create<ServiceSoap>(ServiceEndpoint, timeout);
//    }

//    private INetworkClient<ServiceSoap> GetTestClient()
//    {
//        var africom = Substitute.For<ServiceSoap>();

//        var encryptResponse = new AfriEncryptResponse
//        {
//            Body = new AfriEncryptResponseBody
//            {
//                AfriEncryptResult = "dsfsdfj234"
//            }
//        };

//        africom.AfriEncrypt(Arg.Any<AfriEncryptRequest>()).Returns(encryptResponse);

//        var transferResponse = new AfriTransferResponse
//        {
//            Body = new AfriTransferResponseBody
//            {
//                AfriTransferResult = new Transfer
//                {
//                    THeader = new THeader
//                    {
//                        Code = SuccessCode,
//                        Description = "Test Description",
//                        SystemReferenceNo = "234FDSD"
//                    },
//                    TransferType = new TransferType
//                    {
//                        TransfereeNoBalanceNew = 24.00,
//                        TransfereeNoBalanceOld = 33.00,
//                        SubscriberBalanceOld = 65.00,
//                        SubscriberBalanceNew = 56.00,
//                        ProductName = "Test product name"
//                    }
//                }
//            }
//        };
//        africom.AfriTransfer(Arg.Any<AfriTransferRequest>()).Returns(transferResponse);

//        var fakeFactory = Substitute.For<INetworkClient<ServiceSoap>>();
//        fakeFactory.GetNetwork().Returns(africom);
//        return fakeFactory;
//    }

//    protected override string Name() => "Africom";
//}
