using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Extensions;
using Hot.Ecocash.Application.Common;
using Hot.Ecocash.Application.Common.Interfaces;
using Hot.Ecocash.Domain.Entities;
using Hot.Ecocash.Domain.Enums;
using Hot.Ecocash.Lesotho.Models;

namespace Hot.Ecocash.Lesotho.Services;
public class EcoCashLesothoService : IEcocashService
{
    private readonly IEcocashLesothoAPI service;
    private readonly IEcocashLesothoIdentityAPI tokenService;
    private ServiceOptions options;

    public EcoCashLesothoService(IEcocashLesothoAPI service, IEcocashLesothoIdentityAPI tokenService)
    {
        this.service = service;
        this.tokenService = tokenService;
        options = new();
    }

    public void SetOptions(ServiceOptions options, string APIName)
    {
        this.options = options;
    }

    public EcocashResult ChargeNumber(string MobileNumber, string Reference, decimal Amount)
    {
        return ChargeNumberAsync(MobileNumber, Reference, Amount, "Hot Recharge").Result;
    }

    public EcocashResult ChargeNumber(string MobileNumber, string Reference, decimal Amount, string OnBehalfOf)
    {
        return ChargeNumberAsync(MobileNumber, Reference, Amount, OnBehalfOf).Result;
    }
    public EcocashResult ChargeNumber(string MobileNumber, string Reference, decimal Amount, string OnBehalfOf, Currencies currency)
    {
        return ChargeNumberAsync(MobileNumber, Reference, Amount, OnBehalfOf, null, currency).Result;
    }
    public EcocashResult ChargeNumber(string MobileNumber, string Reference, decimal Amount, string OnBehalfOf, string Remark)
    {
        return ChargeNumberAsync(MobileNumber, Reference, Amount, OnBehalfOf, Remark).Result;
    }

    public EcocashResult RefundTransaction(string MobileNumber, string Reference, decimal Amount, string EcocashReference)
    {
        return RefundTransactionAsync(MobileNumber, Reference, Amount, EcocashReference).Result;
    }

    public EcocashResult QueryTransaction(string MobileNumber, string Reference)
    {
        return QueryTransactionAsync(MobileNumber, Reference).Result;
    }

    public EcocashResult ListTransactions(string MobileNumber)
    {
        return ListTransactionsAsync(MobileNumber).Result;
    }


    public async Task<EcocashResult> ChargeNumberAsync(string MobileNumber, string Reference, decimal Amount, string onBehalfOf, Currencies currency)
    {
        return await ChargeNumberAsync(MobileNumber, Reference, Amount, onBehalfOf, null, currency);
    }

    public async Task<EcocashResult> ChargeNumberAsync(string MobileNumber, string Reference, decimal Amount, string onBehalfOf, string remark = "HotRecharge", Currencies currency = Currencies.ZiG)
    {
        var request = GetChargeRequest(MobileNumber, Reference, Amount, onBehalfOf, remark, currency);
        try
        {
            var token = await tokenService.GetToken(GetAuthRequest());
            var result = await service.PayMerchant(request, token.access_token);
            return HandleResponse(result);
        }
        catch (Exception ex)
        {
            return new EcocashResult(new APIException("Ecocash Error", ex.Message));
        }
    }

    private FormUrlEncodedContent GetAuthRequest()
    {
        return new(new List<KeyValuePair<string, string>>
{
            new KeyValuePair<string, string>("grant_type" , "password"),
            new KeyValuePair<string, string>("password" , options.Password),
            new KeyValuePair<string, string>("username" , options.Username),
        });
    }

    public async Task<EcocashResult> QueryTransactionAsync(string MobileNumber, string Reference)
    {
        var request = new QueryTransactionRequestLesothoModel()
        {
            transactionId = $"HOT{Reference}",
            vendor_code = options.MerchantNumber,
            api_key = options.MerchantPin,
            checksum = "",
        };
        try
        {
            var token = await tokenService.GetToken(GetAuthRequest());
            var result = await service.QueryTransaction(request, token.access_token);
            return HandleResponse(result, request);
        }
        catch (Exception ex)
        {
            return new EcocashResult(new APIException("Ecocash Error", ex.Message));
        }
    }

    public Task<EcocashResult> ListTransactionsAsync(string MobileNumber)
    {
        throw new NotImplementedException();
    }
    public Task<EcocashResult> RefundTransactionAsync(string MobileNumber, string Reference, decimal Amount, string EcocashReference)
    {
        throw new NotImplementedException();
    }

    private PayMerchantRequestLesothoModel GetChargeRequest(string MobileNumber, string Reference, decimal Amount, string onBehalfOf, string Remark = null, Currencies currency = Currencies.ZiG)
    {
        return new()
        {
            amount = Amount.ToString(),
            msisdn = MobileNumber.ToMobile(),
            requestId = $"HOT{Reference}",
            vendor_code = options.MechantCode,
            merchantNumber = options.MerchantNumber,
            merchCode = options.MechantCode,
            api_key = options.MerchantPin,
            callbackurl = options.HotRechargeReturnURL,
            checksum = "",

        };
    }

    private EcocashResult HandleResponse(PayMerchantResponseLesothoModel response)
    {
        return new EcocashResult(MapTransaction(response));
    }
    private EcocashResult HandleResponse(QueryTransactionResponseLesothoModel response, QueryTransactionRequestLesothoModel request)
    {
        return new EcocashResult(MapTransaction(response, request));
    }

    private Transaction MapTransaction(PayMerchantResponseLesothoModel response)
    {
        return new Transaction()
        {
            id = response.trid.ToInt(),
            clientCorrelator = response.exttransactionid,
            ecocashReference = response.txnid,
            merchantCode = options.MechantCode,
            merchantNumber = options.MerchantNumber,
            merchantPin = options.MerchantPin,
            endTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
            notifyUrl = options.HotRechargeReturnURL,
            paymentAmount = new(),
            originalServerReferenceCode = response.trid,
            transactionOperationStatus = response.message,
            referenceCode = response.txnstatus,
        };
    }
    private Transaction MapTransaction(QueryTransactionResponseLesothoModel response, QueryTransactionRequestLesothoModel request)
    {
        return new Transaction()
        {
            id = response.ID,
            clientCorrelator = (response.EXTTRANSACTIONID ?? request.transactionId).Replace("HOT", ""),
            ecocashReference = response.TXNID ?? response.TXNSTATUS,
            merchantCode = options.MechantCode,
            merchantNumber = options.MerchantNumber,
            merchantPin = options.MerchantPin,
            endTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
            notifyUrl = options.HotRechargeReturnURL,
            paymentAmount = new(),
            originalServerReferenceCode = response.TRID,
            transactionOperationStatus = GetTransactionStatus(response),
            serverReferenceCode = response.MESSAGE,
        };
    }

    private static string GetTransactionStatus(QueryTransactionResponseLesothoModel response)
    {
        if (response.TXNSTATUS == "200") return "COMPLETED";
        if (response.TXNSTATUS == "416") return "FAILED";
        if (response.TXNSTATUS == "415") return "FAILED";
        return "PENDING";
    }

    public Task<EcocashResult> RefundTransactionAsync(string MobileNumber, string Reference, decimal Amount, string EcocashReference, Currencies currency)
    {
        throw new NotImplementedException();
    }
}
