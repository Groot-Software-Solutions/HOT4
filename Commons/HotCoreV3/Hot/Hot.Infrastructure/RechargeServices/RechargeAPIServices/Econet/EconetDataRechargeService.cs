using EconetAPI;
using EconetBundleAPI;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models.RechargeServiceModels.Econet;
using Hot.Domain.Entities;
using Hot.Domain.Enums;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Text.Json;

namespace Hot.Infrastructure.RechargeServices.Econet;

public class EconetDataRechargeService : IEconetRechargeDataAPIService
{
    private readonly EconetBundleSoapClient _client;
    private readonly string AppKey;

    public EconetDataRechargeService(IConfiguration configuration)
    {
        var configSection = configuration.GetSection("RechargeServices:EconetData");
        var RemoteUrl = configSection.GetValue("EconetDataAPIUrl", "http://127.0.0.1:8017/econetbundle.asmx");
        _client = new EconetBundleSoapClient(EconetBundleSoapClient.EndpointConfiguration.EconetBundleSoap, RemoteUrl);
        AppKey = configSection.GetValue<string>("AppKey") ?? "Hot263180";
    }

    public async Task<EconetBundleQueryResult> QueryBundles()
    {
        var response = await _client.GetBundlesAsync(AppKey);
        var result = response.Body.GetBundlesResult;

        return new EconetBundleQueryResult(
            result.Length > 0,
              result.Select(b => MapBundleToEconetBundle(b)).ToList()
            );
    }

    public async Task<EconetDataRechargeResult> RechargeDataBundle(string Mobile, string ProductCode, long Reference, Currency currency)
    {
        var currencyCode = currency == Currency.USD ? "1" : "0";
        var response = await _client.RechargeAsync(AppKey, Mobile, ProductCode, Reference, currencyCode);
        var result = response.Body.RechargeResult;
        return new EconetDataRechargeResult()
        {
            Successful = result.StatusCode == 0,
            RawResponseData = JsonSerializer.Serialize(result),
            TransactionResult = result.Description,
            Reference = result.Serial,
            ProductCode = ProductCode,
            StatusCode = result.StatusCode.ToString(),
            InitialWallet = result.InitialWalletBalance,
            FinalWallet = result.FinalWalletBalance,
        };

    }

    private EconetBundleProduct MapBundleToEconetBundle(BundleProduct bundle)
    {
        return new EconetBundleProduct()
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


