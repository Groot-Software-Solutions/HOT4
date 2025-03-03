using Hot.Application.Common.Extensions;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models.RechargeServiceModels.Telecel;
using Hot.Application.Common.Models.RechargeServiceModels.ZESA;
using Hot.Domain.Entities;
using Hot.Domain.Enums;
using Microsoft.Extensions.Configuration;
using NyaradzoAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ZESAAPI;

namespace Hot.Infrastructure.RechargeServices.ZESA
{
    public class ZESARechargeAPIService : IZESARechargeAPIService
    {
        private readonly ZESASoapClient _client;
        private readonly string AppKey;
        public ZESARechargeAPIService(IConfiguration configuration)
        {
            var NetoneConfigSection = configuration.GetSection("RechargeServices:ZESA");
            var RemoteUrl = NetoneConfigSection.GetValue("ZESAAPIUrl", "http://127.0.0.1:8017/ZESA.asmx");
            _client = new ZESASoapClient(ZESASoapClient.EndpointConfiguration.ZESASoap, RemoteUrl);
            AppKey = NetoneConfigSection.GetValue<string>("AppKey") ?? "Hot263180";
        }

        public async Task<ZESAPurchaseTokenResult> PurchaseZesaToken(string MeterNumber, decimal Amount, string Reference, Currency Currency)
        {
            var response = await _client.PurchaseZESATokenByCurrencyAsync(AppKey, MeterNumber, Amount, Reference, Currency.Name() ?? "ZWL");
            var result = response.Body.PurchaseZESATokenByCurrencyResult;

            return new ZESAPurchaseTokenResult()
            {
                Successful = result.ReplyCode == 2,
                RawResponseData = JsonSerializer.Serialize(result),
                TransactionResult = result.ReplyMessage,
                InitialWallet = result.InitialBalance,
                FinalWallet = result.FinalBalance,
                CustomerInfo = MapToCustomerInfo(result.CustomerInfo),
                PurchaseToken = MapToPurchaseToekn(result.PurchaseToken),
                ReturnCode = result.ReplyCode,
            };

        }

        private CustomerInfoModel MapToCustomerInfo(CustomerInfo customerInfo)
        {
            return new()
            {
                Address = customerInfo.Address,
                Currency = customerInfo.Currency,
                CustomerName = customerInfo.CustomerName,
                MeterNumber = customerInfo.MeterNumber,
                Reference = customerInfo.Reference,
            };
        }

        private PurchaseTokenModel MapToPurchaseToekn(PurchaseToken purchaseToken)
        {
            return new()
            {
                Reference = purchaseToken.Reference,
                MeterNumber = purchaseToken.MeterNumber,
                Amount = purchaseToken.Amount,
                Date = DateTime.Now,
                Narrative = purchaseToken.Narrative,
                RawResponse = purchaseToken.RawResponse,
                ResponseCode = purchaseToken.ResponseCode,
                Tokens = MapTokens(purchaseToken.Tokens),
                VendorReference = purchaseToken.VendorReference,
            };
        }

        private List<TokenItemModel> MapTokens(TokenItem[] tokens)
        {
            return tokens
                 .Select(t => new TokenItemModel()
                 {
                     Arrears = t.Arrears,
                     Levy = t.Levy,
                     NetAmount = t.NetAmount,
                     TaxAmount = t.TaxAmount,
                     Token = t.Token,
                     Units = t.Units,
                     ZesaReference = t.ZesaReference,
                 }).ToList();
        }

        public async Task<ZESAAccountQueryResult> QueryZESAAccount(string MeterNumber)
        {
            var response = await _client.GetCustomerInfoAsync(AppKey, MeterNumber);
            var result = response.Body.GetCustomerInfoResult;

            return new ZESAAccountQueryResult()
            {
                Successful = result.ReplyCode == 2,
                TransactionResult = result.ReplyMessage,
                CustomerInfo = MapToCustomerInfo(result.CustomerInfo),
            };
        }


    }
}
