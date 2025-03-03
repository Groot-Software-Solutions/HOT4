using Hot.Application.Common.Extensions;
using Hot.Application.Common.Interfaces;
using Hot.Domain.Entities;
using Hot.Domain.Enums;
using Hot.Ecocash.Domain.Entities;
using Hot.Ecocash.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Ecocash.Application.Common.Extensions
{
    public static class EcocashTransactionExtensions
    {
        public static string GetSmsText(this Transaction request, BankTrx banktrx, Account account, Payment payment, IDbContext dbContext)
        {
            Template template = GetTemplate(banktrx, dbContext);

            return banktrx.IsAPITransaction()
                    ? banktrx.GetClientSMS(request.ecocashReference, account.ReferredBy ?? "", account.Balance, template)
                    : banktrx.GetDealerSMS(account, payment, template);
        }

        private static Template GetTemplate(BankTrx banktrx, IDbContext dbContext)
        {
            var templateResponse = dbContext
                            .Templates.Get(
                                banktrx.IsAPITransaction() ? 503 : 502
                                );
            if (templateResponse.IsT1) throw templateResponse.AsT1;
            var template = templateResponse.AsT0;
            return template;
        }

        public static Payment CreatePayment(this Transaction request, BankTrx banktrx, Access access, string LastUser = "Ecocash")
        {
            var payment = new Payment()
            {
                AccountId = access.AccountId,
                Amount = banktrx.Amount,
                LastUser = LastUser,
                PaymentDate = banktrx.TrxDate,
                PaymentSourceId = (byte)PaymentSources.EcoCash,
                PaymentTypeId = (byte)banktrx.GetPaymentTypeId(),
                Reference = $"Ecocash Payment Successful. Ecocash Ref: {request.ecocashReference}",
            };

            return payment;
        }

        public static Transaction GetAnonymizedTransaction(this Transaction request)
        {
            request.merchantNumber = "0772929223";
            request.merchantPin = "0000";
            return request;
        }
        public static EcocashAccounts GetEcocashAccountType(this BankTrx bankTrx)
        {
            if (bankTrx.IsZesaTransaction()) return EcocashAccounts.ZESAAccount;
            if (bankTrx.IsAPITransaction()) return EcocashAccounts.APIUserAccount;
            if (bankTrx.IsUSDTransaction()) return EcocashAccounts.FCAAccount;
            if (bankTrx.IsUSDUtilityTransaction()) return EcocashAccounts.FCAAccount;

            return EcocashAccounts.MainAccount;

        }
    }
}
