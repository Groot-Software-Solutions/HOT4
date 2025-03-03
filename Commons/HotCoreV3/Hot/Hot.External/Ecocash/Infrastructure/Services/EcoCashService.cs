using Hot.Ecocash.Application.Common;
using Hot.Ecocash.Application.Common.Interfaces;
using Hot.Ecocash.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hot.Application.Common.Interfaces;
using OneOf;
using Hot.Application.Common.Exceptions;
using Hot.Ecocash.Domain.Enums;
using Hot.Application.Common.Extensions;

namespace Hot.Ecocash.Infrastructure.Services
{
    public class EcoCashService : IEcocashService
    {
        readonly IAPIService service;

        private ServiceOptions options = new();

        public EcoCashService(IAPIService service)
        {
            this.service = service;
        }

        public void SetOptions(ServiceOptions options, string APIName)
        {
            this.options = options;
            service.APIName = APIName;
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
            var result = RefundTransactionAsync(MobileNumber, Reference, Amount, EcocashReference, Currencies.ZiG).Result;
            return result;
        }
        public EcocashResult RefundTransaction(string MobileNumber, string Reference, decimal Amount, string EcocashReference, Currencies currency)
        {
            return RefundTransactionAsync(MobileNumber, Reference, Amount, EcocashReference, currency).Result;
        }
        public Task<EcocashResult> RefundTransactionAsync(string MobileNumber, string Reference, decimal Amount, string EcocashReference)
        {
            return RefundTransactionAsync(MobileNumber, Reference, Amount, EcocashReference);
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

        public async Task<EcocashResult> ChargeNumberAsync(string MobileNumber, string Reference, decimal Amount, string onBehalfOf, string? Remark = null, Currencies currency = Currencies.ZiG)
        {
            string address = $"transactions/amount";
            RequestItem request = GetChargeRequest(MobileNumber, Reference, Amount, onBehalfOf, Remark, currency);
            return await SendRequest(address, request);
        }

        public async Task<EcocashResult> RefundTransactionAsync(string MobileNumber, string Reference, decimal Amount, string EcocashReference, Currencies currency = Currencies.ZiG)
        {
            var address = $"transactions/refund";
            var request = new RequestRefund(EcocashReference, options)
            {
                clientCorrelator = Reference,
                endUserId = MobileNumber,
                referenceCode = $"HOT-{Reference}",
                transactionOperationStatus = "Charged",
                paymentAmount = new PaymentAmount(Amount, "Refund", "Hot Recharge", currency),
                currencyCode = (Enum.GetName(typeof(Currencies), currency) ?? "").Replace("_", "-"),
            };
            return await SendRequest(address, request);
        }

        public async Task<EcocashResult> QueryTransactionAsync(string MobileNumber, string Reference)
        {
            //var address = $"tel%3A%2B{MobileNumber}/transactions/amount/{Reference}";
            var address = $"{MobileNumber.ToMSISDN()}/transactions/amount/{Reference}";
            var response = await service.Get<Transaction>(address);
            return HandleResponse(response);

        }

        public async Task<EcocashResult> ListTransactionsAsync(string MobileNumber)
        {
            //var address = $"tel%3A%2B{MobileNumber}/transactions/";
            var address = $"{MobileNumber.ToMSISDN()}/transactions/";
            var response = await service.Get<List<Transaction>>(address);
            return HandleResponse(response);
        }


        private async Task<EcocashResult> SendRequest(string address, RequestItem request)
        {
            var response = await service.Post<Transaction, RequestItem>(address, request);
            return HandleResponse(response);
        }

        private async Task<EcocashResult> SendRequest(string address, RequestRefund request)
        {
            var response = await service.Post<Transaction, RequestRefund>(address, request);
            return HandleResponse(response);
        }

        private static EcocashResult HandleResponse(OneOf<List<Transaction>, APIException, string> response)
        {
            return response.Match(
                transactions => new EcocashResult(transactions),
                error => new EcocashResult(error),
                data => new EcocashResult(data)
                );
        }

        private static EcocashResult HandleResponse(OneOf<Transaction, APIException, string> response)
        {
            var result = response.Match(
                              transaction => new EcocashResult(transaction),
                              error => new EcocashResult(error),
                              data => new EcocashResult(data)
                              );
            return result;
        }

        private RequestItem GetChargeRequest(string MobileNumber, string Reference, decimal Amount, string onBehalfOf, string? Remark = null, Currencies currency = Currencies.ZiG)
        {
            return new RequestItem(options)
            {
                clientCorrelator = Reference,
                endUserId = MobileNumber,
                referenceCode = $"HOT-{Reference}",
                transactionOperationStatus = "Charged",
                paymentAmount = new PaymentAmount(Amount, "Hot Recharge", onBehalfOf, currency),
                remark = Remark ?? "Airtime",
                currencyCode = (Enum.GetName(typeof(Currencies), currency) ?? "").Replace("_", "-"),

            };
        }


    }
}
