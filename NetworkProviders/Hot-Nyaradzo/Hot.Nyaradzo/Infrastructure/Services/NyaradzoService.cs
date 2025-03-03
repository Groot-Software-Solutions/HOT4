using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Interfaces;
using Hot.Domain.Enums;
using Hot.Nyaradzo.Application.Common.Interfaces;
using Hot.Nyaradzo.Application.Common.Models;
using Hot.Nyaradzo.Domain.Entities;
using OneOf;

namespace Hot.Nyaradzo.Infrastructure.Services
{
    public class NyaradzoService : INyaradzoService
    {
        readonly IAPIService service;
        //private ServiceOptions options;

        public NyaradzoService(IAPIService service)
        {
            this.service = service;
            this.service.APIName = "Nyaradzo";
        }


        public NyaradzoResultModel AccountEnquiry(string policyNumber, string Reference)
        {
            return AccountEnquiryAsync(policyNumber, Reference).Result;
        }

        public NyaradzoResultModel TransactionReversal(string Reference)
        {
            return TransactionReversalAsync(Reference).Result;
        }

        public NyaradzoResultModel ProcessPayment(string policyNumber, string Reference, decimal amountPaid, decimal monthlyPremium, int numberOfMonthsPaid, DateTime date, Currency currency)
        {
            return ProcessPaymentAsync(policyNumber, Reference, amountPaid, monthlyPremium, numberOfMonthsPaid, date,currency).Result;
        }


        public async Task<NyaradzoResultModel> AccountEnquiryAsync(string policyNumber, string Reference)
        {
            int numberOfMonths = 1;
            string address = $"api/payments/generic/account_enquiry" +
                $"?policyNumber={policyNumber}&numberOfMonths={numberOfMonths}&sourceReference={Reference}";
            var response = await service.Get<AccountSummary>(address);
            return HandleResponse(response);
        }

        public async Task<NyaradzoResultModel> TransactionReversalAsync(string reference)
        {
            string address = $"api/payments/generic/reversal?sourceReference={reference}";
            var response = await service.Get<ReversalResult>(address);
            return HandleResponse(response);
        }

        public async Task<NyaradzoResultModel> ProcessPaymentAsync(string PolicyNumber, string Reference, decimal AmountPaid, decimal MonthlyPremium, int NumberOfMonthsPaid, DateTime date,Currency currency)
        {
            string address = $"api/payments/generic/payment_processing";
            PaymentRequest request = new(PolicyNumber, Reference, AmountPaid, MonthlyPremium, NumberOfMonthsPaid, date, currency);
            var response = await service.Post<PaymentResult, PaymentRequest>(address, request);
            return HandleResponse(response);

        }

        private static NyaradzoResultModel HandleResponse(OneOf<PaymentResult, APIException, string> response)
        {
            return response.Match(
                paymentResult => new NyaradzoResultModel(paymentResult),
                error => new NyaradzoResultModel(error),
                data => new NyaradzoResultModel(data)
                );
        }

        private static NyaradzoResultModel HandleResponse(OneOf<AccountSummary, APIException, string> response)
        {
            return response.Match(
                accountSummary => new NyaradzoResultModel(accountSummary),
                error => new NyaradzoResultModel(error),
                data => new NyaradzoResultModel(data)
                );
        }

        private static NyaradzoResultModel HandleResponse(OneOf<ReversalResult, APIException, string> response)
        {
            return response.Match(
                reversalResult => new NyaradzoResultModel(reversalResult),
                error => new NyaradzoResultModel(error),
                data => new NyaradzoResultModel(data)
                );
        }





    }
}
