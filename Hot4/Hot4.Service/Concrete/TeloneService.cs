using EcocashHotLib.Data.Enums;
using Hot4.Core.Enums;
using Hot4.Core.Settings;
using Hot4.DataModel.Models;
using Hot4.Service.Abstract;
using Hot4.ViewModel;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using NSubstitute;
using System.Data;
using System.Reflection;
using TelOneAPI;
using ZesaAPI;

namespace Hot4.Service.Concrete
{
    public class TeloneService : NetworkBase<TeloneSoap>
    {
        private readonly IRechargePrepaidService _rechargePrepaidService;
        private readonly IRechargeService _rechargeService;
        private readonly TeloneSettings _teloneSettings;
        private readonly IPinBatchService _pinBatchService;
        private readonly IPinService _pinService;
        public TeloneService(
            string ServiceEndpoint,
            bool IsTestMode,
            string ApplicationName,
            IRechargePrepaidService rechargePrepaidService,
            IRechargeService rechargeService,
            IOptions<NetworkSettings> networkSettings,
            IPinBatchService pinBatchService,
            IPinService pinService
            ) : base (ServiceEndpoint,IsTestMode,ApplicationName)
        {
            _rechargePrepaidService = rechargePrepaidService;
            _rechargeService = rechargeService;
            _teloneSettings = networkSettings.Value.Telone ?? throw new ArgumentNullException(nameof(networkSettings.Value.Telone));
            _pinBatchService = pinBatchService;
            _pinService = pinService;
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
                // Debit not possible on Telone 
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
        public async Task <ServiceRechargeResponse> BulkRechargePrepaid(RechargeModel rechargeModel, int numberOfDays = 90)
        {
            ApplyDiscountRules(rechargeModel);
            RechargePrepaidModel rechargePrepaidModel = CreateRechargeObject(rechargeModel);
           await _rechargePrepaidService.AddRechargePrepaid(rechargePrepaidModel);

            if (rechargePrepaidModel.DebitCredit)
                BulkAdslProductRecharge(rechargeModel, rechargePrepaidModel, numberOfDays);
            else
            {
                // Debit not possible on Telone 
                rechargePrepaidModel.Narrative = "Bundle Debit not possible";
                rechargePrepaidModel.ReturnCode = "099";
                rechargeModel.StateId = (byte)RechargeStates.Failure;
            }
            Console.WriteLine("Saving prepaid");
            await _rechargePrepaidService.AddRechargePrepaid(rechargePrepaidModel);
            Console.WriteLine("Saving Recharge");
            await _rechargeService.AddRechargeWithOutSmsDetails(rechargeModel);
            return new ServiceRechargeResponse(rechargePrepaidModel);
        }
        private async void CreditSubscriber(RechargeModel rechargeModel, RechargePrepaidModel rechargePrepaidModel, int numberOfDays)
        {
            var client = GetNetworkClient(_teloneSettings.TimeoutPeriod);
            try
            {
                Console.WriteLine("Crediting");
                var serviceClient = client.GetNetwork();
                RechargeAccountResponse Response;
                if (rechargeModel.BrandId == 43)
                    Response = await serviceClient.RechargeAccountAdslUSDAsync(_teloneSettings.HotAPIKey, rechargeModel.Mobile, (int)rechargeModel.Denomination, (int)rechargeModel.RechargeId);
                else
                    Response = await serviceClient.RechargeAccountAdslAsync(_teloneSettings.HotAPIKey, rechargeModel.Mobile, (int)rechargeModel.Denomination, (int)rechargeModel.RechargeId);

                //  xLog_Data.WriteJsonToConsole(Response);

                Console.WriteLine("Response from Telone " + Response.ToString());
                if (!HasTimeoutErrors(Response.ReplyMessage))
                    SetRechargePrepaid(ref rechargeModel, ref rechargePrepaidModel, numberOfDays, Response);
                else
                    // QueryRecharge(recharge, rechargePrepaid, numberOfDays, client)hriow  
                    throw new Exception(Response.ReplyMessage);
                if (rechargeModel.IsSuccessFul)
                {
                    var list = new List<Voucher>();
                    list.AddRange(Response.Result.Voucher);
                    SavePins(list, rechargeModel);
                }
            }
            catch (Exception ex)
            {
                client.Abort();
                Console.WriteLine("Credit Exception");
                //  xLog_Data.Save(ApplicationName, GetType().Name, MethodBase.GetCurrentMethod().Name, ex, SqlConn.ConnectionString, "RechargeID", recharge.RechargeID);
                rechargeModel.StateId = (byte)RechargeStates.Failure;
                rechargePrepaidModel.Narrative = "TelOneAPI Error: " + ex.Message;
            }

            finally
            {
                client.Close();
            }
        }
        private async void BulkAdslProductRecharge(RechargeModel rechargeModel, RechargePrepaidModel rechargePrepaidModel, int numberOfDays)
        {
            var client = GetNetworkClient(_teloneSettings.TimeoutPeriod);
            try
            {
                Console.WriteLine("Crediting");
                var serviceClient = client.GetNetwork();
                var Response = await serviceClient.BulkAdslPurchaseBroadbandAsync(_teloneSettings.HotAPIKey, (int)rechargeModel.Denomination, (int)rechargeModel.Quantity, (int)rechargeModel.RechargeId);
               // xLog_Data.WriteJsonToConsole(Response);

                Console.WriteLine("Response from Telone " + Response.ToString());
                if (!HasTimeoutErrors(Response.ReplyMessage))
                    SetRechargePrepaid(ref rechargeModel, ref rechargePrepaidModel, numberOfDays, Response);
                else
                  //  throw new Exception(Response.ReplyMessage);
                if (rechargeModel.IsSuccessFul)
                    SavePins(Response.Result.Vouchers.ToList(), rechargeModel);
            }
            catch (Exception ex)
            {
                client.Abort();
                Console.WriteLine("Credit Exception");
                //  xLog_Data.Save(ApplicationName, GetType().Name, MethodBase.GetCurrentMethod().Name, ex, null, "RechargeID", rechargeMode.RechargeId);
                rechargeModel.StateId = (byte)RechargeStates.Failure;
                rechargePrepaidModel.Narrative = "TelOneAPI Error: " + ex.Message;
            }

            finally
            {
                client.Close();
            }
        }
        private static bool HasTimeoutErrors(string Response)
        {
            if (Response.Contains("request channel timed out"))
                return true;
            if (Response.Contains("task was canceled"))
                return true;
            return false;
        }
        private static void SetRechargePrepaid(ref RechargeModel rechargeModel, ref RechargePrepaidModel rechargePrepaidModel, int numberOfDays, RechargeAccountResponse Response)
        {
            rechargePrepaidModel.Narrative = Response.ReplyMessage + " Raw:" + JsonConvert.SerializeObject(Response.Result);
            rechargePrepaidModel.ReturnCode = Response.ReplyCode.ToString();
            rechargePrepaidModel.FinalBalance = 0;
            rechargePrepaidModel.InitialBalance = 0;
            rechargePrepaidModel.FinalWallet = (decimal)Response.Result.Balance;
            rechargePrepaidModel.Window = DateTime.Now;
            rechargePrepaidModel.DebitCredit = true;
            rechargePrepaidModel.RechargeId = rechargeModel.RechargeId;
            rechargePrepaidModel.Window = DateTime.Now.AddDays(numberOfDays);
            rechargeModel.StateId = Response.ReplyCode == (int)RechargeStates.Success ? (byte)RechargeStates.Success : (byte)RechargeStates.Failure;
            rechargePrepaidModel.InitialWallet = (decimal)Response.Result.Balance + (rechargeModel.IsSuccessFul ? rechargeModel.Amount : 0);
            rechargeModel.RechargeDate = DateTime.Now;
            rechargePrepaidModel.Data = 0.ToString();
        }
        private static void SetRechargePrepaid(ref RechargeModel rechargeModel, ref RechargePrepaidModel rechargePrepaidModel, int numberOfDays, BulkPurchaseBroadbandResponse Response)
        {
            rechargePrepaidModel.Narrative = Response.ReplyMessage + " Raw:" + JsonConvert.SerializeObject(Response.Result);
            rechargePrepaidModel.ReturnCode = Response.ReplyCode.ToString();
            rechargePrepaidModel.FinalBalance = 0;
            rechargePrepaidModel.InitialBalance = 0;
            rechargePrepaidModel.FinalWallet = (decimal)Response.Result.Balance;

            rechargePrepaidModel.Window = DateTime.Now;
            rechargePrepaidModel.DebitCredit = true;
            rechargePrepaidModel.RechargeId = rechargeModel.RechargeId;
            rechargePrepaidModel.Window = DateTime.Now.AddDays(numberOfDays);
            rechargeModel.StateId = Response.ReplyCode == (int)RechargeStates.Success ? (byte)RechargeStates.Success : (byte)RechargeStates.Failure;
            rechargeModel.RechargeDate = DateTime.Now;
            rechargePrepaidModel.Data = 0.ToString();
        }
        public async Task <CustomerAccountResponse> VerifyAccount(string AccountId)
        {
            var client = GetNetworkClient(_teloneSettings.TimeoutPeriod);
            try
            {
                var serviceClient = client.GetNetwork();
                var Response = await serviceClient.VerifiyUserAccountAsync(_teloneSettings.HotAPIKey, AccountId);

                if (Response.ReplyCode ==  (byte)RechargeStates.Success)
                    return Response.Result;
                return new CustomerAccountResponse()
                {
                    AccountNumber = AccountId,
                    ResponseCode = Response.ReplyCode.ToString(),
                    AccountName = "Not Found"
                };
            }
            catch (Exception ex)
            {
                client.Abort();
                Console.WriteLine("Credit Exception");
               // xLog_Data.Save(ApplicationName, GetType().Name, MethodBase.GetCurrentMethod().Name, ex, SqlConn.ConnectionString, "RechargeID", 0);
                return null;
            }
            finally
            {
                client.Close();
            }
        }
        public async Task<decimal> GetEndUserBalance(string AccountId)
        {
            var client = GetNetworkClient(_teloneSettings.TimeoutPeriod);
            try
            {
                var serviceClient = client.GetNetwork();
                var Response = await serviceClient.EndUserVoiceBalanceAsync(_teloneSettings.HotAPIKey, AccountId);

                if (Response.ReplyCode == (int)RechargeStates.Success)
                    return Response.Balance;
                return -1;
            }
            catch (Exception ex)
            {
                client.Abort();
                Console.WriteLine("Credit Exception");
               // xLog_Data.Save(ApplicationName, GetType().Name, MethodBase.GetCurrentMethod().Name, ex, SqlConn.ConnectionString, "RechargeID", 0);
                return default(Decimal);
            }
            finally
            {
                client.Close();
            }
        }
        public  async Task<List<BroadbandProductItem>> QueryEVDStock(bool IsUsd = false)
        {
            var client = GetNetworkClient(_teloneSettings.TimeoutPeriod);
            try
            {
                var serviceClient = client.GetNetwork();

                var Response =  IsUsd ? await serviceClient.GetAvailableBundlesUSDAsync(_teloneSettings.HotAPIKey) : await serviceClient.GetAvailableBundlesAsync(_teloneSettings.HotAPIKey);
                var items = new List<BroadbandProductItem>();

                if (Response.ReplyCode == (int)RechargeStates.Success)
                    items = Response.List.ToList();

                return items;
            }
            catch (Exception ex)
            {
                client.Abort();
                Console.WriteLine("Credit Exception");
              //  xLog_Data.Save(ApplicationName, GetType().Name, MethodBase.GetCurrentMethod().Name, ex, SqlConn.ConnectionString, "RechargeID", 0);
                return null;
            }
            finally
            {
                client.Close();
            }
        }
        public async Task< ServiceRechargeResponse> PayAccountBill(RechargeModel rechargeModel, int numberOfDays = 90)
        {
            ApplyDiscountRules(rechargeModel);
            RechargePrepaidModel rechargePrepaidModel = CreateRechargeObject(rechargeModel);
          await  _rechargePrepaidService.AddRechargePrepaid(rechargePrepaidModel);

            if (rechargePrepaidModel.DebitCredit)
              await  PayBill(rechargeModel, rechargePrepaidModel, numberOfDays);
            else
            {
                // Debit not possible on Telone 
                rechargePrepaidModel.Narrative = "Bundle Debit not possible";
                rechargePrepaidModel.ReturnCode = "099";
                rechargeModel.StateId = (byte)RechargeStates.Failure;
            }
            Console.WriteLine("Saving prepaid");
            await _rechargePrepaidService.AddRechargePrepaid(rechargePrepaidModel);
            Console.WriteLine("Saving Recharge");
           await _rechargeService.AddRechargeWithOutSmsDetails(rechargeModel);
            return new ServiceRechargeResponse(rechargePrepaidModel);
        }
        private async Task< ServiceRechargeResponse> PayBill(RechargeModel rechargeModel, RechargePrepaidModel rechargePrepaidModel, int numberOfDays)
        {
            var client = GetNetworkClient(_teloneSettings.TimeoutPeriod);
            try
            {
                Console.WriteLine("Crediting");
                var serviceClient = client.GetNetwork();
                var Response = await serviceClient.PayBillAsync(_teloneSettings.HotAPIKey, rechargeModel.Mobile, rechargeModel.Amount, (int)rechargeModel.RechargeId);
              //  xLog_Data.WriteJsonToConsole(Response);

                Console.WriteLine("Response from Telone " + Response.ToString());
                if (!HasTimeoutErrors(Response.ReplyMessage))
                    SetRechargePrepaid(rechargeModel,rechargePrepaidModel, numberOfDays, Response);
                else
                    throw new Exception(Response.ReplyMessage);
                return null; // check
            }
            catch (Exception ex)
            {
                client.Abort();
                Console.WriteLine("Credit Exception");
                // xLog_Data.Save(ApplicationName, GetType().Name, MethodBase.GetCurrentMethod().Name, ex, SqlConn.ConnectionString, "RechargeID", recharge.RechargeID);
                rechargeModel.StateId = (byte)RechargeStates.Failure;
                rechargePrepaidModel.Narrative = "TelOneAPI Error: " + ex.Message;
                return null; // check
            }

            finally
            {
                client.Close();
            }
        }
        private void SetRechargePrepaid(RechargeModel rechargeModel, RechargePrepaidModel rechargePrepaidModel, int numberOfDays, PayAccountBillResponse response)
        {
            rechargePrepaidModel.Narrative = response.ReplyMessage + " Raw:" + JsonConvert.SerializeObject(response.Result);
            rechargePrepaidModel.ReturnCode = response.ReplyCode.ToString();
            rechargePrepaidModel.FinalBalance = 0;
            rechargePrepaidModel.InitialBalance = 0;
            rechargePrepaidModel.FinalWallet = (decimal)response.Result.Balance;

            rechargePrepaidModel.Window = DateTime.Now;
            rechargePrepaidModel.DebitCredit = true;
            rechargePrepaidModel.RechargeId = rechargeModel.RechargeId;
            rechargePrepaidModel.Window = DateTime.Now.AddDays(numberOfDays);
            rechargeModel.StateId = response.ReplyCode == (int)RechargeStates.Success ?  (byte)RechargeStates.Success: (byte)RechargeStates.Failure;
            rechargeModel.RechargeDate = DateTime.Now;
            rechargePrepaidModel.Data = 0.ToString();
        }
        public async Task <ServiceRechargeResponse> RechargeVoipAccount(RechargeModel rechargeModel, int numberOfDays = 90)
        {
            ApplyDiscountRules(rechargeModel);
            RechargePrepaidModel rechargePrepaidModel = CreateRechargeObject(rechargeModel);
            await _rechargePrepaidService.AddRechargePrepaid(rechargePrepaidModel);

            if (rechargePrepaidModel.DebitCredit)
               await RechargeVoip(rechargeModel, rechargePrepaidModel, numberOfDays);
            else
            {
                // Debit not possible on Telone 
                rechargePrepaidModel.Narrative = "Bundle Debit not possible";
                rechargePrepaidModel.ReturnCode = "099";
                rechargeModel.StateId = (byte)RechargeStates.Failure;
            }
            Console.WriteLine("Saving prepaid");
            await _rechargePrepaidService.AddRechargePrepaid(rechargePrepaidModel);
            Console.WriteLine("Saving Recharge");
            await _rechargeService.AddRechargeWithOutSmsDetails(rechargeModel);
            return new ServiceRechargeResponse(rechargePrepaidModel);
        }
        private async Task<ServiceRechargeResponse> RechargeVoip(RechargeModel rechargeModel, RechargePrepaidModel rechargePrepaidModel, int numberOfDays)
        {
            var client = GetNetworkClient(_teloneSettings.TimeoutPeriod);
            try
            {
                Console.WriteLine("Crediting");
                var serviceClient = client.GetNetwork();
                var Response = await serviceClient.RechargeAccountVoipAsync(_teloneSettings.HotAPIKey, rechargeModel.Mobile, rechargeModel.Amount, (int)rechargeModel.RechargeId);
              //  xLog_Data.WriteJsonToConsole(Response);

                Console.WriteLine("Response from Telone " + Response.ToString());
                if (!HasTimeoutErrors(Response.ReplyMessage))
                    SetRechargePrepaid( rechargeModel, rechargePrepaidModel, numberOfDays, Response);
                else 
                    throw new Exception(Response.ReplyMessage);
                return null; // chcek
            }
            catch (Exception ex)
            {
                client.Abort();
                Console.WriteLine("Credit Exception");
                //xLog_Data.Save(ApplicationName, GetType().Name, MethodBase.GetCurrentMethod().Name, ex, SqlConn.ConnectionString, "RechargeID", recharge.RechargeID);
                rechargeModel.StateId = (byte)RechargeStates.Failure;
                rechargePrepaidModel.Narrative = "TelOneAPI Error: " + ex.Message;
                return null;
            }

            finally
            {
                client.Close();
            }
        }
        private void SetRechargePrepaid(RechargeModel rechargeModel, RechargePrepaidModel rechargePrepaidModel, int numberOfDays, RechargeVoipResponse response)
        {
            rechargePrepaidModel.Narrative = response.ReplyMessage + " Raw:" + JsonConvert.SerializeObject(response.Result);
            rechargePrepaidModel.ReturnCode = response.ReplyCode.ToString();
            rechargePrepaidModel.FinalBalance = 0;
            rechargePrepaidModel.InitialBalance = 0;
            rechargePrepaidModel.FinalWallet =(decimal)response.Result.Balance;

            rechargePrepaidModel.Window = DateTime.Now;
            rechargePrepaidModel.DebitCredit = true;
            rechargePrepaidModel.RechargeId = rechargeModel.RechargeId;
            rechargePrepaidModel.Window = DateTime.Now.AddDays(numberOfDays);
            rechargeModel.StateId = response.ReplyCode == (int)RechargeStates.Success ?  (byte)RechargeStates.Success : (byte)RechargeStates.Failure;
            rechargeModel.RechargeDate = DateTime.Now;
            rechargePrepaidModel.Data = 0.ToString();
        }
        private async void SavePins(List<Voucher>  vouchers, RechargeModel rechargeModel)
        {
                try
                {
                PinBatchRecord pinBatchRecord = new PinBatchRecord
                    {
                        PinBatch = $"TelOneAPI-PinBatch-{DateTime.Now}",
                        PinBatchTypeId = (byte)PinBatchTypesEnum.TelOne,
                    };
                await _pinBatchService.AddPinBatch(pinBatchRecord);
                    var pinRecords = new List<PinRecord>();
                    foreach (var voucher in vouchers)
                    {
                    PinRecord pinRecord = GetPin(voucher, rechargeModel, pinBatchRecord);
                        if (!string.IsNullOrEmpty(pinRecord.Pin))
                        {
                      await  _pinService.AddPin(pinRecord);
                        pinRecords.Add(pinRecord);
                        }
                    }
                    var RP = new DataTable();
                    RP.Columns.Add("RechargeID");
                    RP.Columns.Add("PinID");
                    foreach (var pin in pinRecords)
                    {
                        var dr = RP.NewRow();
                        dr["PinID"] = pin.PinId;
                        dr["RechargeID"] = rechargeModel.RechargeId;
                        RP.Rows.Add(dr);
                    }
                    var objbulk = new SqlBulkCopy(null, SqlBulkCopyOptions.Default, null);
                    objbulk.DestinationTableName = "tblRechargePin";
                    objbulk.ColumnMappings.Add("RechargeID", "RechargeID");
                    objbulk.ColumnMappings.Add("PinID", "PinID");
                    objbulk.WriteToServer(RP);
                }
                catch (Exception ex)
                {

                }
            
        }
        private PinRecord GetPin(Voucher body, RechargeModel rechargeModel, PinBatchRecord pinBatchRecord)
        {
            PinRecord pinRecord = new PinRecord();
            try
            {
                //   pinRecord.BrandId = new xBrand();
                pinRecord.BrandId = rechargeModel.BrandId;

                pinRecord.Pin = body.Pin;
                pinRecord.PinRef = $"{body.CardNumber}|{body.SerialNumber}|{body.BatchNumber}";
                pinRecord.PinBatchId = pinBatchRecord.PinBatchId;
                pinRecord.PinExpirty = DateTime.Now.AddYears(1);
                //  pinRecord.PinState = new xPinState();
                pinRecord.PinStateId = (byte)PinStateType.SoldFileExport;
                pinRecord.PinValue = (decimal)rechargeModel.Amount;
            }
            catch (Exception ex)
            {
                return new PinRecord();
            }

            return pinRecord;
        }
        public override INetworkClient<TeloneSoap> GetNetworkClient(int timeout = 30000)
        {
            if (IsTestMode)
                return GetTestClient();
            return NetworkClientFactory.Create<TeloneSoap>(ServiceEndpoint, timeout);
        }
        private INetworkClient<TeloneSoap> GetTestClient()
        {
            TeloneSoap client = Substitute.For<TeloneSoap>();
            // Change Response Code to anything else but 1 to test failure
            var response = new TelOneAPI.RechargeAccountResponse()
            {
                ReplyCode = 2,
                ReplyMessage = "Narrative test",
                Reference = "hjkk"
            };
            client.RechargeAccountAdslAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>()).Returns(response);
            var fakeFactory = Substitute.For<INetworkClient<TeloneSoap>>();
            fakeFactory.GetNetwork().Returns(client);
            return fakeFactory;
        }

    }
}
