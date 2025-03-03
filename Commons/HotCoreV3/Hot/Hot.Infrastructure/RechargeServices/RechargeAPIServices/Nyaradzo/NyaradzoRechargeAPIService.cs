using Hot.Application.Common.Extensions;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models.RechargeServiceModels.Nyaradzo;
using Hot.Domain.Enums;
using Microsoft.Extensions.Configuration;
using NyaradzoAPI;
using System.Text.Json;

namespace Hot.Infrastructure.RechargeServices.Nyaradzo;

public class NyaradzoRechargeAPIService : INyaradzoRechargeAPIService
{

    private readonly NyaradzoSoapClient _client;
    private readonly string AppKey;
    public NyaradzoRechargeAPIService(IConfiguration configuration)
    {
        var NetoneConfigSection = configuration.GetSection("RechargeServices:Nyaradzo");
        var RemoteUrl = NetoneConfigSection.GetValue("NyaradzoAPIUrl", "http://127.0.0.1:8017/nyaradzo.asmx");
        _client = new NyaradzoSoapClient(NyaradzoSoapClient.EndpointConfiguration.NyaradzoSoap, RemoteUrl);
        AppKey = NetoneConfigSection.GetValue<string>("AppKey") ?? "Hot263180";
    }

    public async Task<NyaradzoResult> ProcessPayment(NyaradzoPaymentRequest Payment, Currency currency)
    {
        var response = await _client.ProcessPaymentAsync(AppKey, Payment.PolicyNumber, Payment.Reference, Payment.AmountPaid);
        var result = response.Body.ProcessPaymentResult;
        return new NyaradzoResult()
        {
            Account = MapResult(result.Item),
            Successful = result.ValidResponse,
            TransactionResult = result.Result.Message,
            RawResponseData = JsonSerializer.Serialize(result),
        };
    }

    public async Task<NyaradzoResult> QueryAccount(string PolicyNumber)
    {
        var response = await _client.AccountQueryAsync(AppKey, PolicyNumber);
        var result = response.Body.AccountQueryResult;
        return new NyaradzoResult()
        {
            Account = MapResult(result.Item),
            Successful = result.ValidResponse,
            TransactionResult = result.Result.Message,
            RawResponseData = JsonSerializer.Serialize(result),
        };
    }

    public async Task<NyaradzoResult> Reversal(string Reference)
    {
        var response = await _client.RefundAsync(AppKey, Reference);
        var result = response.Body.RefundResult;
        return new NyaradzoResult()
        {
            Account = MapResult(result.Item),
            Successful = result.ValidResponse,
            TransactionResult = result.Result.Message,
            RawResponseData = JsonSerializer.Serialize(result),
        };
    }



    private static NyaradzoAccountSummary MapResult(AccountSummary item)
    {
        var hasCurrency  = Enum.TryParse(item.CurrencyCode, out Currency currency);
        return new()
        {
            AmountToBePaid = item.AmountToBePaid,
            Balance = item.Balance,
            BankCode = item.BankCode,
            DateCreated = DateTime.Now,
            Id = item.Id,
            MonthlyPremium = item.MonthlyPremium,
            NumberOfMonths = item.NumberOfMonths,
            PolicyHolder = item.PolicyHolder,
            PolicyNumber = item.PolicyNumber,
            ResponseCode = item.ResponseCode,
            ResponseDescription = item.ResponseDescription,
            SourceReference = item.SourceReference,
            Status = item.Status,
            TxnRef = item.TxnRef,
            currency = hasCurrency ? currency : Currency.ZWG
        };
}

}

