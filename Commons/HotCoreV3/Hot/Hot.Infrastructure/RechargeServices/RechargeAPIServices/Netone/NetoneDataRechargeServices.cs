
using Hot.Application.Common.Extensions;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models.RechargeServiceModels.NetOne;
using Hot.Domain.Enums;
using Microsoft.Extensions.Configuration;
using NetoneAPI; 
using System.Text.Json; 

namespace Hot.Infrastructure.RechargeServices.RechargeAPIServices.Netone;

public class NetoneDataRechargeServices: INetoneRechargeDataAPIService
{
    private readonly NetoneSoapClient _client;
    private readonly string AppKey;
    public NetoneDataRechargeServices(IConfiguration configuration)
    {
        var NetoneConfigSection = configuration.GetSection("RechargeServices:Netone");
        var RemoteUrl = NetoneConfigSection.GetValue("NetoneAPIUrl", "http://127.0.0.1:8017/netone.asmx");
        _client = new NetoneSoapClient(NetoneSoapClient.EndpointConfiguration.NetoneSoap, RemoteUrl);
        AppKey = NetoneConfigSection.GetValue<string>("AppKey") ?? "Hot263180";
    }

    public async Task<NetoneBundleQueryResult> QueryBundles()
    {
        var response = await _client.GetBundlesAsync(AppKey);
        var result = response.Body.GetBundlesResult;

        return new NetoneBundleQueryResult(
            result.Length > 0,
              result.Select(b => MapBundleToEconetBundle(b)).ToList()
            );
    }

    public async Task<NetoneDataRechargeResult> RechargeDataBundle(string Mobile, string ProductCode, long Reference,decimal Amount, Currency currency)
    { 
        return currency == Currency.USD 
            ? await RechargeUSD(Mobile, ProductCode, Reference, Amount)
            : await RechargeZWL(Mobile, ProductCode, Reference,Amount);

    }

    private async Task<NetoneDataRechargeResult> RechargeUSD(string Mobile, string ProductCode, long Reference, decimal Amount)
    {
        var response = await _client.RechargeUSDBundleAsync(AppKey, Mobile, ProductCode, Amount.ToString(), Reference);
        var result = response.Body.RechargeUSDBundleResult;
        var isValidBalance = result.WalletBalance.TryParse(out decimal walletbalance);
        return new NetoneDataRechargeResult()
        {
            Successful = result.ReplyCode == 2,
            RawResponseData = JsonSerializer.Serialize(result),
            TransactionResult = result.ReplyMessage,
            InitialWallet = isValidBalance ? walletbalance : 0,
            FinalWallet = isValidBalance ? walletbalance : 0,
            ReplyCode = result.ReplyCode,
        };
    }
    private async Task<NetoneDataRechargeResult> RechargeZWL(string Mobile, string ProductCode, long Reference, decimal Amount)
    {
        var response = await _client.RechargeBundleAsync(AppKey, Mobile, ProductCode, Amount.ToString(), Reference);
        var result = response.Body.RechargeBundleResult;
        var isValidBalance = result.WalletBalance.TryParse(out decimal walletbalance);
        return new NetoneDataRechargeResult()
        {
            Successful = result.ReplyCode == 2,
            RawResponseData = JsonSerializer.Serialize(result),
            TransactionResult = result.ReplyMessage,
            InitialWallet = isValidBalance ? walletbalance : 0,
            FinalWallet = isValidBalance ? walletbalance : 0,
            ReplyCode = result.ReplyCode,
        };
    }


    private NetoneBundleProduct MapBundleToEconetBundle(BundleProduct bundle)
    {
        return new NetoneBundleProduct()
        {
            Amount = bundle.Amount,
            BrandID = bundle.BrandId,
            BundleID = bundle.BundleId,
            Description = bundle.Description,
            Enabled = true,
            Name = bundle.Name,
            Network = bundle.Network,
            ProductCode = bundle.ProductCode,
            ValidityPeriod = bundle.ValidityPeriod,
        };
    }

}
