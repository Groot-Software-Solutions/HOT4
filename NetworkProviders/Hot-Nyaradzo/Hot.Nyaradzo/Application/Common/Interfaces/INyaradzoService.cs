using Hot.Domain.Enums;
using Hot.Nyaradzo.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Nyaradzo.Application.Common.Interfaces
{
    public interface INyaradzoService
    { 

        public NyaradzoResultModel AccountEnquiry(string policyNumber, string Reference);
        public NyaradzoResultModel TransactionReversal(string Reference);
        public NyaradzoResultModel ProcessPayment(string policyNumber, string Reference, decimal amountPaid, decimal monthlyPremium, int numberOfMonthsPaid, DateTime date, Currency currency);
        public Task<NyaradzoResultModel> AccountEnquiryAsync(string policyNumber, string Reference);
        public Task<NyaradzoResultModel> TransactionReversalAsync(string Reference);
        public Task<NyaradzoResultModel> ProcessPaymentAsync(string policyNumber, string Reference, decimal amountPaid, decimal monthlyPremium, int numberOfMonthsPaid, DateTime date, Currency currency);
       
    }
}
