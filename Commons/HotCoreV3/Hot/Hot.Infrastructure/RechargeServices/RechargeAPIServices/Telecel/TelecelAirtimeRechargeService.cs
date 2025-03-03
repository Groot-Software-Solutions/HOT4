using Hot.Application.Common.Extensions;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models.RechargeServiceModels.NetOne;
using Hot.Application.Common.Models.RechargeServiceModels.Telecel;
using Hot.Domain.Enums;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TelecelAPI;

namespace Hot.Infrastructure.RechargeServices.Telecel;
public class TelecelAirtimeRechargeService : ITelecelRechargeAPIService
{
    private readonly TelecelSoapClient _client;
    private readonly string AgentCode;
    private readonly string MPin;
    public TelecelAirtimeRechargeService(IConfiguration configuration)
    {
        var ConfigSection = configuration.GetSection("RechargeServices:Telecel");
        var RemoteUrl = ConfigSection.GetValue("TelecelAPIUrl", "http://127.0.0.1:8017/Telecel.asmx");

        _client = new TelecelSoapClient(TelecelSoapClient.EndpointConfiguration.TelecelSoap, RemoteUrl);
        AgentCode = ConfigSection.GetValue<string>("AgentCode") ?? "733357030";
        MPin = ConfigSection.GetValue<string>("MPin") ?? "5394C86F977989489AFC65FEFC525CE1";
    }


    public async Task<TelecelRechargeResult> Recharge(string Mobile, decimal Amount, string Reference, Currency currency)
    {
        if (currency == Currency.USD) return await RechargeUSD(Mobile, Amount, Reference);
        return await RechargeZWL(Mobile, Amount, Reference);
    }

    public async Task<TelecelRechargeResult> RechargeZWL(string Mobile, decimal Amount, string Reference)
    {

        var response = await _client.JuiceRechargeAsync(AgentCode, MPin, Mobile, Amount.ToString(), Reference);
        var result = response.Body.JuiceRechargeResult;
        var hasinitialBalance = result.pre_balance.TryParse<decimal>(out var initialbalance);
        var hasfinalBalance = result.post_balance.TryParse<decimal>(out var finalbalance);
        var hasinitialWalletBalance = result.preWalletBalance.TryParse<decimal>(out var initialWalletBalance);
        var hasfinalWalletBalance = result.walletBalance.TryParse<decimal>(out var finalWalletBalance);

        return new TelecelRechargeResult()
        {
            Successful = result.resultcode == "0",
            RawResponseData = JsonSerializer.Serialize(result),
            TransactionResult = result.responseValue,
            InitialBalance = hasinitialBalance ? initialbalance : 0,
            FinalBalance = hasfinalBalance ? finalbalance : 0,
            FinalWallet = hasfinalWalletBalance ? finalWalletBalance : 0,
            InitialWallet = hasinitialWalletBalance ? initialWalletBalance : 0,
            Narrative = result.responseValue,
            ReturnCode = result.resultcode
        };
    }

    public async Task<TelecelRechargeResult> RechargeUSD(string Mobile, decimal Amount, string Reference)
    {

        var response = await _client.JuiceRechargeUSDAsync(AgentCode, MPin, Mobile, Amount.ToString(), Reference);
        var result = response.Body.JuiceRechargeUSDResult;
        var hasinitialBalance = result.pre_balance.TryParse<decimal>(out var initialbalance);
        var hasfinalBalance = result.post_balance.TryParse<decimal>(out var finalbalance);
        var hasinitialWalletBalance = result.preWalletBalance.TryParse<decimal>(out var initialWalletBalance);
        var hasfinalWalletBalance = result.walletBalance.TryParse<decimal>(out var finalWalletBalance);

        return new TelecelRechargeResult()
        {
            Successful = result.resultcode == "0",
            RawResponseData = JsonSerializer.Serialize(result),
            TransactionResult = result.responseValue,
            InitialBalance = hasinitialBalance ? initialbalance : 0,
            FinalBalance = hasfinalBalance ? finalbalance : 0,
            FinalWallet = hasfinalWalletBalance ? finalWalletBalance : 0,
            InitialWallet = hasinitialWalletBalance ? initialWalletBalance : 0,
            Narrative = result.responseValue,
            ReturnCode = result.resultcode
        };
    }


}

