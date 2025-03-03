using Hot.Application.Common.Extensions;
using Hot.Domain.Enums;
using Hot.Nyaradzo.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Nyaradzo.Domain.Entities
{
    public class PaymentRequest
    {
        public string sourceReference { get; set; }
        public string date { get; set; }
        public string policyNumber { get; set; }
        public decimal amountPaid { get; set; }
        public int numberOfMonths { get; set; }
        public decimal monthlyPremium { get; set; }
        public string currency { get; set; }

        public PaymentRequest(string PolicyNumber, string Reference, decimal AmountPaid, decimal MonthlyPremium, int NumberOfMonthsPaid, DateTime Date, Currency Currency)
        {
            amountPaid = AmountPaid;
            date = $"{Date:yyyy-MM-ddTHH:mm:ss.fff}";
            monthlyPremium = MonthlyPremium;
            numberOfMonths = NumberOfMonthsPaid;
            policyNumber = PolicyNumber;
            sourceReference = $"{Reference}";
            currency = Currency.Name() ?? "ZiG";
        }
    }
}
