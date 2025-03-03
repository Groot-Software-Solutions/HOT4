using AutoMapper;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models.RechargeServiceModels.Nyaradzo;
using Hot.Domain.Enums;
using Hot.Nyaradzo.Application.Common.Interfaces;
using Hot.Nyaradzo.Domain.Entities;

namespace Hot.Nyaradzo.Infrastructure.Services
{
    public class NyaradzoAPIService : INyaradzoRechargeAPIService
    {
        private readonly INyaradzoServiceFactory nyaradzo;
        private readonly IMapper mapper;

        public NyaradzoAPIService(INyaradzoServiceFactory nyaradzo, IMapper mapper)
        {
            this.nyaradzo = nyaradzo;
            this.mapper = mapper;
        }
        public async Task<NyaradzoResult> ProcessPayment(NyaradzoPaymentRequest Payment, Currency currency)
        {
            var service = nyaradzo.GetService();
            var result = await service.ProcessPaymentAsync(
                Payment.PolicyNumber,
                Payment.Reference,
                Payment.AmountPaid,
                Payment.MonthlyPremium,
                Payment.NumberOfMonthsPaid,
                Payment.Date,
                currency);
            if (result.ValidResponse)
            {
                try
                {
                    var response = await service.AccountEnquiryAsync(Payment.PolicyNumber, Guid.NewGuid().ToString());
                    if (response.ValidResponse) result.Item = response.Item;
                }
                catch (Exception) { }
            }
            return Map(result);
        }

        public async Task<NyaradzoResult> QueryAccount(string PolicyNumber)
        {
            var service = nyaradzo.GetService();
            var result = await service.AccountEnquiryAsync(PolicyNumber, Guid.NewGuid().ToString());

            return Map(result);
        }


        public async Task<NyaradzoResult> Reversal(string Reference)
        {
            var service = nyaradzo.GetService();
            var result = await service.TransactionReversalAsync(Reference);
            return Map(result);
        }

        private NyaradzoResult Map(Application.Common.Models.NyaradzoResultModel data)
        {
            var result = new NyaradzoResult()
            {
                Account = MapAccount(data.Item),
                Successful = data.Item == null ? !string.IsNullOrEmpty(data.Result?.Message) : data.Item?.status == "ACTIVE",
                RawResponseData = data.ResponseData,
                TransactionResult = data.ValidResponse
                    ? !string.IsNullOrEmpty(data.Result.Message) ? data.Result?.Message : data.Item?.responseDescription
                    : data.ErrorData,
            };
            return result;
        }

        private NyaradzoAccountSummary MapAccount(AccountSummary item)
        {
            var hasCurrency = Enum.TryParse(item.currencyCode, out Currency currency); 
            return new()
            {
                currency = hasCurrency ? currency : Currency.ZWG,
                AmountToBePaid = item.amountToBePaid,
                Balance = item.balance,
                BankCode = item.bankCode,
                DateCreated = item.dateCreated,
                Id = item.id,
                MonthlyPremium = item.monthlyPremium,
                NumberOfMonths = item.numberOfMonths,
                PolicyHolder = item.policyHolder,
                PolicyNumber = item.policyNumber,
                ResponseCode = item.responseCode,
                ResponseDescription = item.responseDescription,
                SourceReference = item.sourceReference,
                Status = item.status,
                TxnRef = item.txnRef,
            };
        }

    }
}
