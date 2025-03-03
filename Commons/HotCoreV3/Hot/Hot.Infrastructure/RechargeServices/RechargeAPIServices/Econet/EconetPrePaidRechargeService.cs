using EconetBundleAPI;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models.RechargeServiceModels.Econet;
using Hot.Domain.Enums;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Hot.Infrastructure.RechargeServices.Econet;

public class EconetPrePaidRechargeService : IEconetRechargePrepaidAPIService
{
    private readonly EconetBundleSoapClient _client;
    private readonly string AppKey;

    public EconetPrePaidRechargeService(IConfiguration configuration)
    {
        var configSection = configuration.GetSection("RechargeServices:EconetData");
        var RemoteUrl = configSection.GetValue("EconetDataAPIUrl", "http://10.10.31.60:8017/econetbundle.asmx");
        _client = new EconetBundleSoapClient(EconetBundleSoapClient.EndpointConfiguration.EconetBundleSoap, RemoteUrl);
        AppKey = configSection.GetValue<string>("AppKey") ?? "Hot263180";
    }

    public async Task<EconetRechargeResult> RechargeAirtime(string Mobile, decimal Amount, long Reference, Currency currency)
    {
        var currencyCode = currency == Currency.USD ? "1" : "0";
        var response = await _client.RechargeAirtimeAsync(AppKey, Mobile, Amount, Reference, currencyCode);
        var result = response.Body.RechargeAirtimeResult;
        return new EconetRechargeResult()
        {
            Successful = result.StatusCode == 0,
            RawResponseData = JsonSerializer.Serialize(result),
            TransactionResult = result.Description,
            ResponseCode = result.StatusCode.ToString(),
            InitialWallet = result.InitialWalletBalance,
            FinalWallet = result.FinalWalletBalance,
        };

    }


}

