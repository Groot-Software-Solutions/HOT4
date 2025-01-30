using AutoMapper;
using EcocashHotLib.Data.Enums;
using Hot4.Core.Constant;
using Hot4.Core.Enums;
using Hot4.Core.Helper;
using Hot4.Core.Settings;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using NSubstitute;
using System.Data;
using System.Net.Http;
using System.Reflection;
using ZesaAPI;

namespace Hot4.Service.Concrete
{
    public class ZesaService : NetworkBase<ZESASoap> , IZesaService
    {
        private readonly ZesaSettings _zesaSettings;
        private readonly IRechargeService _rechargeService;
        private readonly IMapper Mapper;
        private readonly string _applicationName;
        private readonly IPinBatchService _pinBatchService;
        private readonly IPinService _pinService;
        private readonly IRechargePrepaidService _rechargePrepaidService;
        private readonly ITemplateRepository _templateRepository;

        public ZesaService(
           string ServiceEndpoint,
           bool IsTestMode,
           string ApplicationName,
        string applicationName,
            IRechargeService rechargeService,
            IMapper mapper,    
            IPinBatchService pinBatchService,
            IPinService pinService,
            IOptions<NetworkSettings> networkSettings,
            IRechargePrepaidService rechargePrepaidService,
            ITemplateRepository templateRepository
            ) : base(ServiceEndpoint , IsTestMode , ApplicationName)
        {
            _zesaSettings = networkSettings.Value.Zesa ?? throw new ArgumentNullException(nameof(networkSettings.Value.Zesa));
            _rechargeService = rechargeService;
            Mapper = mapper;
            _pinBatchService = pinBatchService;
            _pinService = pinService;
            _rechargePrepaidService = rechargePrepaidService;
            _templateRepository = templateRepository;
        }
        public async Task<ServiceRechargeResponse> CompleteZesaAsync(RechargeModel rechargeModel, PurchaseToken tokenResponse, long smsId)
        {
            var rechargePrepaid = await _rechargePrepaidService.GetRechargePrepaidById(rechargeModel.RechargeId);

            var response = new PurchaseTokenResponse
            {
                CustomerInfo = new CustomerInfo(),
                FinalBalance = rechargePrepaid.FinalBalance,
                InitialBalance = rechargePrepaid.InitialBalance,
                PurchaseToken = tokenResponse,
                ReplyCode = tokenResponse.ResponseCode == "00" ? 2 : 3,
                Reference = rechargePrepaid.Reference,
                ReplyMessage = tokenResponse.Narrative
            };
            try
            {
                SetRechargePrepaid(rechargeModel, rechargePrepaid, 90, response);

                if (rechargeModel != null)
                {
                     SavePins(response, rechargeModel);
                     SaveFees(response, rechargeModel, smsId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Credit Exception");
           //     xLog_Data.Save(ApplicationName, GetType().Name, MethodBase.GetCurrentMethod().Name, ex, null, "RechargeID", rechargeModel.RechargeId);
                rechargeModel.StateId = (byte)RechargeStates.Failure;
            }
            await _rechargePrepaidService.AddRechargePrepaid(rechargePrepaid);
            await _rechargeService.AddRechargeWithOutSmsDetails(rechargeModel);
            return new ServiceRechargeResponse(rechargePrepaid);
        }
        private async void CreditSubscriber(RechargeModel rechargeModel, RechargePrepaidModel rechargePrepaidModel, int numberOfDays)
        {
            PurchaseZESATokenByCurrencyRequest creditRequest = CreateCreditRequest(rechargeModel, rechargePrepaidModel, numberOfDays);
            var networkClient = GetNetworkClient(_zesaSettings.TimeoutPeriod);
            try
            {
                Console.WriteLine("Crediting");
                var client = networkClient.GetNetwork();
                var Initresponse = await client.PurchaseZESATokenByCurrencyAsync(creditRequest);
                   var Response = Initresponse.Body.PurchaseZESATokenByCurrencyResult;
             //   xLog_Data.WriteJsonToConsole(Response);

                Console.WriteLine("Response from Netone " + Response.ToString());
                if (!HasTimeoutErrors(Response.ReplyMessage))
                    SetRechargePrepaid(rechargeModel, rechargePrepaidModel, numberOfDays, Response);
                else

                    throw new Exception(Response.ReplyMessage);
                if (rechargeModel.IsSuccessFul == true)
                    SavePins(Response, rechargeModel);
            }
            catch (Exception ex)
            {
                networkClient.Abort();
                Console.WriteLine("Credit Exception");
                //  xLog_Data.Save(ApplicationName, GetType().Name, MethodBase.GetCurrentMethod().Name, ex, SqlConn.ConnectionString, "RechargeID", recharge.RechargeID);
                rechargeModel.StateId = (byte)RechargeStates.Failure;
                rechargePrepaidModel.Narrative = "NetoneAPI Error: " + ex.Message;
            }
            finally
            {
                networkClient.Close();
            }
        }
        private async void SaveFees(PurchaseTokenResponse response, RechargeModel _rechargeModel, long smsId)
        {
            var rechargeModel = new RechargeModel()
            {
                StateId = 2,
                AccessId = _rechargeModel.AccessId,
                Amount = 0,
                BrandId = (byte)Brands.ZETDCFees,
                Discount = 0,
                Mobile = _rechargeModel.Mobile,
                RechargeDate = DateTime.Now
            };

            await _rechargeService.AddRecharge(rechargeModel, smsId);

            RechargePrepaidModel rechargePrepaidModel = new RechargePrepaidModel()
            {
                RechargeId = rechargeModel.RechargeId,
                DebitCredit = rechargeModel.Amount >= 0,
                Reference = rechargeModel.Mobile,
                FinalBalance = -1,
                InitialBalance = -1,
                Narrative = $"Recharge ID: {_rechargeModel.RechargeId} ",
                ReturnCode = "-1",
                Data = _rechargeModel.RechargeId.ToString(),
                FinalWallet = response.FinalBalance,
                InitialWallet = response.InitialBalance
            };

            await _rechargePrepaidService.AddRechargePrepaid(rechargePrepaidModel);
        }
        private async void SavePins(PurchaseTokenResponse response, RechargeModel rechargeModel)
        {
            {
                try
                {
                    PinBatchRecord pinBatchRecord = new PinBatchRecord
                    {
                        PinBatch = $"ZESA-PinBatch-{DateTime.Now:MMddyyyy}",
                        PinBatchTypeId = (byte)PinBatchTypesEnum.ZESA,
                        BatchDate = DateTime.Now,
                        //PinBatchType = new xPinBatchType();
                    };
                    await _pinBatchService.AddPinBatch(pinBatchRecord);
                    List<PinRecord> pinRecords = GetPin(response.PurchaseToken, rechargeModel, pinBatchRecord);
                    foreach (PinRecord pinRecord in pinRecords)
                    {
                        if (!string.IsNullOrEmpty(pinRecord.Pin))
                        {
                            try
                            {
                               await _pinService.AddPin(pinRecord);
                                DataTable rp = new DataTable();
                                rp.Columns.Add("RechargeID");
                                rp.Columns.Add("PinID");

                                DataRow dr = rp.NewRow();
                                dr["PinID"] = pinRecord.PinId;
                                dr["RechargeID"] = rechargeModel.RechargeId;
                                rp.Rows.Add(dr);
                                using (SqlBulkCopy objbulk = new SqlBulkCopy(null, SqlBulkCopyOptions.Default, null))
                                {
                                    objbulk.DestinationTableName = "tblRechargePin";
                                    objbulk.ColumnMappings.Add("RechargeID", "RechargeID");
                                    objbulk.ColumnMappings.Add("PinID", "PinID");
                                    objbulk.WriteToServer(rp);
                                }
                            }
                            catch (Exception ex)
                            {
                                // Handle exception if needed
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    // Optionally throw exception or log
                }
            }
        }
        private List<PinRecord> GetPin(PurchaseToken body, RechargeModel rechargeModel, PinBatchRecord pinBatchRecord)
        {
            List<PinRecord> pinRecords = new List<PinRecord>();
            foreach (var pin in body.Tokens)
            {
                try
                {
                    var pinRecord = new PinRecord
                    {
                        BrandId = rechargeModel.BrandId,
                        Pin = pin.Token,
                        PinRef = pin.ZesaReference,
                        PinBatchId = pinBatchRecord.PinBatchId,
                        PinExpirty = DateTime.Now.AddYears(1),
                        PinStateId = (byte)PinStateType.SoldHotBanking,
                        PinValue = (decimal)body.Amount
                    };

                    // pinRecord.PinStateId = (byte)PinStateType.SoldHotBanking;
                    pinRecords.Add(pinRecord);
                }
                catch (Exception ex)
                {
                    // Optionally log or handle the exception
                    // For example: Log.Error(ex);
                    // Return an empty xPin or handle as required
                }
            }

            return pinRecords;
        }
        private PurchaseZESATokenByCurrencyRequest CreateCreditRequest(RechargeModel rechargeModel, RechargePrepaidModel iRechargePrepaid, int numberOfDays)
        {
            return new PurchaseZESATokenByCurrencyRequest()
            {
                Body = new PurchaseZESATokenByCurrencyRequestBody()
                {
                    Reference = rechargeModel.RechargeId.ToString(),
                    Amount = rechargeModel.Amount,
                    MeterNumber = rechargeModel.Mobile,
                    AppKey = _zesaSettings.HotAPIKey,
                    Currency = rechargeModel.BrandId == (byte)Brands.ZETDCUSD ? ZesaConstants.USD : ZesaConstants.ZWL
                }
            };
        }
        public override ServiceRechargeResponse RechargePrepaid(RechargeModel rechargeModel, int numberOfDays = 90)
        {
            ApplyDiscountRules(rechargeModel);
            RechargePrepaidModel rechargePrepaidModel = CreateRechargeObject(rechargeModel);
            _rechargePrepaidService.AddRechargePrepaid(rechargePrepaidModel);

            if (rechargePrepaidModel.DebitCredit)
                CreditSubscriber(rechargeModel, rechargePrepaidModel, numberOfDays);
            else
            {
                // Debit not possible on Netone 
                rechargePrepaidModel.Narrative = "Bundle Debit not possible";
                rechargePrepaidModel.ReturnCode = "099";
                rechargeModel.StateId = (byte)RechargeStates.Failure;
            }
            Console.WriteLine("Saving prepaid");
            _rechargePrepaidService.AddRechargePrepaid(rechargePrepaidModel);
            Console.WriteLine("Saving Recharge");
            _rechargeService.AddRechargeWithOutSmsDetails(rechargeModel);
            return new ServiceRechargeResponse(rechargePrepaidModel);
        }
        private static bool HasTimeoutErrors(string Response)
        {
            if (Response.Contains("request channel timed out"))
                return true;
            if (Response.Contains("task was canceled"))
                return true;
            return false;
        }
        private void SetRechargePrepaid(RechargeModel  rechargeModel, RechargePrepaidModel rechargePrepaidModel, int numberOfDays, PurchaseTokenResponse response)
        {
            rechargePrepaidModel.Narrative = JsonConvert.SerializeObject(response);
            rechargePrepaidModel.FinalBalance = response.FinalBalance;
            rechargePrepaidModel.InitialBalance = response.InitialBalance;
            rechargePrepaidModel.Window = DateTime.Now.AddDays(numberOfDays);
            rechargeModel.StateId = response.ReplyCode == (int)RechargeStates.Success ? (byte)RechargeStates.Success : (byte)RechargeStates.Failure;
            rechargeModel.RechargeDate = DateTime.Now;
        }
        public async Task<CustomerInfo> CustomerInfoRequest(RechargePrepaidModel  rechargePrepaidModel, RechargeModel rechargeModel)
        {
            GetCustomerInfoRequest request = new GetCustomerInfoRequest()
            {
                Body = new GetCustomerInfoRequestBody()
                {
                    AppKey =_zesaSettings.HotAPIKey,
                    MeterNumber = rechargeModel.Mobile
                }
            };
            var netowrkclient = GetNetworkClient();
            try
            {
                Console.WriteLine("Querying");
                var client = netowrkclient.GetNetwork();
                var Initresponse = await client.GetCustomerInfoAsync(request);
                var response = Initresponse.Body.GetCustomerInfoResult;

              //  xLog_Data.WriteJsonToConsole(response);
                netowrkclient.Close();
                Console.WriteLine("Response from ZESA " + response.ToString());
                rechargePrepaidModel.ReturnCode = response.ReplyCode.ToString();
                rechargePrepaidModel.Reference = GetReference(request.Body.MeterNumber);
                rechargePrepaidModel.FinalBalance = 0;
                rechargePrepaidModel.InitialBalance = 0;
                rechargePrepaidModel.Narrative = $"{response.ReplyMessage} - {JsonConvert.SerializeObject(response.CustomerInfo)}";
                rechargePrepaidModel.DebitCredit = true;
                rechargePrepaidModel.RechargeId = rechargeModel.RechargeId;
                rechargeModel.StateId = response.ReplyCode == (byte)RechargeStates.Success ? (byte)RechargeStates.Success : (byte)RechargeStates.Failure;
                rechargeModel.RechargeDate = DateTime.Now;
                return response.CustomerInfo;
            }
            catch (Exception ex)
            {
                netowrkclient.Abort();
                Console.WriteLine("Balance Exception");
                //  xLog_Data.Save(ApplicationName, GetType().Name, MethodBase.GetCurrentMethod().Name, ex, SqlConn.ConnectionString, "RechargeID", recharge.RechargeID);
                rechargeModel.StateId = (byte)RechargeStates.Failure;
                rechargePrepaidModel.Narrative = "ZESAAPI Error: " + ex.Message;
                var template = await _templateRepository.GetTemplateById((int)TemplateName.NetworkWebserviceUnavailable);
               // var template = xTemplateAdapter.SelectRow(xTemplate.Templates.NetworkWebserviceUnavailable, SqlConn);
                var text = template.TemplateText.Replace("%NETWORK%", ZesaConstants.ZESA);
                // throw new HotRequestException(TemplateName.NetworkWebserviceUnavailable, text);
                return null;
            }
        }
        public override INetworkClient<ZESASoap> GetNetworkClient(int timeout = 30000)
        {
            if (IsTestMode)
                return GetTestClient();
            return NetworkClientFactory.Create<ZESASoap>(ServiceEndpoint, timeout);
        }
        private INetworkClient<ZESASoap> GetTestClient()
        {
            ZESASoap netone = Substitute.For<ZESASoap>();
            var response = new PurchaseZESATokenResponse()
            {
                Body = new PurchaseZESATokenResponseBody()
                {
                    PurchaseZESATokenResult = new PurchaseTokenResponse()
                    {
                        ReplyCode = 2,
                        ReplyMessage = "Narrative test",
                        Reference = "Tes Ref",
                        PurchaseToken = new PurchaseToken()
                        {
                            Amount = 1,
                            Tokens = new List<TokenItem>().ToArray()
                        }
                    }
                }
            };
            netone.PurchaseZESATokenAsync(Arg.Any<PurchaseZESATokenRequest>()).Returns(response);
            var fakeFactory = Substitute.For<INetworkClient<ZESASoap>>();
            fakeFactory.GetNetwork().Returns(netone);
            return fakeFactory;
        }
    }
}
    