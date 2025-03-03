using Hot.Application.Common.Extensions;
using Hot.Application.Common.Interfaces.RechargeServices; 
using Hot.Application.Common.Models.RechargeServiceModels.NetOne; 
using Hot.Domain.Enums;
using Microsoft.Extensions.Configuration;
using NetoneAPI;
using System.Text.Json;

namespace Hot.Infrastructure.RechargeServices.Netone;
public class NetoneAirtimeRechargeService : INetOneRechargeAPIService
{
    private readonly NetoneSoapClient _client;
    private readonly string AppKey;
    public NetoneAirtimeRechargeService(IConfiguration configuration)
    {
        var NetoneConfigSection = configuration.GetSection("RechargeServices:Netone");
        var RemoteUrl = NetoneConfigSection.GetValue("NetoneAPIUrl", "http://127.0.0.1:8017/netone.asmx");
        _client = new NetoneSoapClient(NetoneSoapClient.EndpointConfiguration.NetoneSoap, RemoteUrl);
        AppKey = NetoneConfigSection.GetValue<string>("AppKey") ?? "Hot263180";
    }

    public async Task<NetOneRechargeResult> QueryEndUserBalance(string Mobile)
    {
        var response = await _client.UserBalanceAsync(AppKey, Mobile);
        var result = response.Body.UserBalanceResult;
        return new NetOneRechargeResult()
        {
            Successful = result.ReplyCode == 2,
            RawResponseData = JsonSerializer.Serialize(result),
            TransactionResult = result.ReplyMsg,
            FinalBalance = result.MobileBalance.Parse<decimal>(),
            Narrative = result.ReplyMsg,
            ReturnCode = result.ReplyCode.ToString(),
        };
    }


    public async Task<NetOneRechargeResult> Recharge(string Mobile, decimal Amount, long Reference, Currency currency)
    {
        if (currency == Currency.USD) return await RechargeUSD(Mobile, Amount, Reference);
        return await RechargeZWG(Mobile, Amount, Reference);
    }

    public async Task<NetOneRechargeResult> RechargeZWG(string Mobile, decimal Amount, long Reference)
    {
        var response = await _client.RechargeMobileAsync(AppKey, Mobile, Amount.ToString(), Reference);
        var result = response.Body.RechargeMobileResult;
        var hasFinalBalance = result.WalletBalance.TryParse(out decimal FinalBalance);

        return new NetOneRechargeResult()
        {
            Successful = result.ReplyCode == 2,
            RawResponseData = JsonSerializer.Serialize(result),
            TransactionResult = string.IsNullOrWhiteSpace(result.ReplyMessage) ? result.ReplyMsg : result.ReplyMessage,
            InitialBalance = result.InitialBalance,
            FinalBalance = result.FinalBalance,
            FinalWallet = hasFinalBalance ? FinalBalance : 0,
            InitialWallet = hasFinalBalance ? FinalBalance - result.Amount : 0,
            Narrative = string.IsNullOrWhiteSpace(result.ReplyMessage) ? result.ReplyMsg : result.ReplyMessage,
            RechargeId = result.RechargeID,
            ReturnCode = result.ReplyCode.ToString(),
        };
    }
    public async Task<NetOneRechargeResult> RechargeUSD(string Mobile, decimal Amount, long Reference)
    {
        var response = await _client.RechargeMobileUSDAsync(AppKey, Mobile, Amount.ToString(), Reference);
        var result = response.Body.RechargeMobileUSDResult;
        var hasFinalBalance = result.WalletBalance.TryParse(out decimal FinalBalance);

        return new NetOneRechargeResult()
        {
            Successful = result.ReplyCode == 2,
            RawResponseData = JsonSerializer.Serialize(result),
            TransactionResult = string.IsNullOrWhiteSpace(result.ReplyMessage) ? result.ReplyMsg : result.ReplyMessage,
            InitialBalance = result.InitialBalance,
            FinalBalance = result.FinalBalance,
            FinalWallet = hasFinalBalance ? FinalBalance : 0,
            InitialWallet = hasFinalBalance ? FinalBalance - result.Amount : 0,
            Narrative = string.IsNullOrWhiteSpace(result.ReplyMessage) ? result.ReplyMsg : result.ReplyMessage,
            RechargeId = result.RechargeID,
            ReturnCode = result.ReplyCode.ToString(),
        };
    }




}


