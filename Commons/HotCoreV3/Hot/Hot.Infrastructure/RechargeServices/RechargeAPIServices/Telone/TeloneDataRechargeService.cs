using EconetBundleAPI;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models.RechargeServiceModels.Nyaradzo;
using Hot.Application.Common.Models.RechargeServiceModels.Telone;
using Hot.Domain.Enums;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TelOneAPI;

namespace Hot.Infrastructure.RechargeServices.Telone;
public class TeloneDataRechargeService : ITeloneDataAPIService
{
    private readonly TeloneSoapClient _client;
    private readonly string AppKey;

    public TeloneDataRechargeService(IConfiguration configuration)
    {
        var configSection = configuration.GetSection("RechargeServices:Telone");
        var RemoteUrl = configSection.GetValue("TeloneAPIUrl", "http://127.0.0.1:8017/telone.asmx");
        _client = new TeloneSoapClient(TeloneSoapClient.EndpointConfiguration.TeloneSoap, RemoteUrl);
        AppKey = configSection.GetValue<string>("AppKey") ?? "Hot263180";
    }

    public async Task<TeloneBundleQueryResult> QueryBundles(Currency currency)
    {
        var result = currency == Currency.USD
            ? await _client.GetAvailableBundlesUSDAsync(AppKey)
            : await _client.GetAvailableBundlesAsync(AppKey);

        return new TeloneBundleQueryResult(
            result.ReplyCode == 2,
             result.List.Select(b => MapBundleToTeloneBundle(b, currency)).ToList()
            );
    }

    public async Task<TeloneRechargeResult> RechargeDataBundle(string Mobile, int ProductId, int Reference, Currency currency)
    {
        var result = currency == Currency.USD
            ? await _client.RechargeAccountAdslUSDAsync(AppKey, Mobile, ProductId, Reference)
            : await _client.RechargeAccountAdslAsync(AppKey, Mobile, ProductId, Reference);

        return new TeloneRechargeResult()
        {
            Successful = result.ReplyCode == 2,
            RawResponseData = JsonSerializer.Serialize(result),
            TransactionResult = result.ReplyMessage,
            Reference = result.Result.OrderNumber,
            ResponseCode = result.Result.ResponseCode,
            Vouchers = result.Result.Voucher.Select(v => MaptoVoucher(v)).ToList(),
            InitialWallet = (decimal)result.Result.Balance,
            FinalWallet = (decimal)result.Result.Balance,

        };

    }

    public async Task<TeloneCustomerResult> QueryAccount(string AccountNumber)
    {
        var response = await _client.VerifiyUserAccountAsync(AppKey, AccountNumber);
        var result = response;
        return new TeloneCustomerResult()
        {
            Account = MapResult(result.Result),
            Successful = result.ReplyCode == 2,
            TransactionResult = result.ReplyMessage,
            RawResponseData = JsonSerializer.Serialize(result),
        };
    }

    private static TeloneCustomer MapResult(CustomerAccountResponse result)
    {
        return new() {
            AccountName=result.AccountName,
            AccountNumber = result.AccountNumber,
            AccountAddress = result.AccountAddress,
            ResponseDescription = result.ResponseDescription
        };
    }

    private static TeloneVoucher MaptoVoucher(Voucher voucher)
    {
        return new()
        {
            BatchNumber = voucher.BatchNumber,
            CardNumber = voucher.CardNumber,
            Pin = voucher.Pin,
            ProductId = voucher.ProductId,
            SerialNumber = voucher.SerialNumber,
        };
    }

    private static TeloneBundleProduct MapBundleToTeloneBundle(BroadbandProductItem bundle, Currency currency)
    {
        return new TeloneBundleProduct()
        {
            Amount = bundle.Price,
            Description = bundle.Description,
            Name = bundle.Name,
            ProductId = bundle.ProductId,
            BrandId = MapBrand(bundle.Description, currency),
        };
    }

    private static Brands MapBrand(string description, Currency currency)
    {
        if (currency == Currency.USD) return Brands.TeloneUSD;
        return description switch
        {
            "Voice" => Brands.TeloneVoice,
            "LTE" => Brands.TeloneLTE,
            _ => Brands.TeloneBroadband
        };

    }
}

