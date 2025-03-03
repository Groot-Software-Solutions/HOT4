using System;
using System.Collections.Generic;
using System.Text;

namespace TelOne.Application.Common.Models
{
    public class QueryCustomerVoiceBalanceResponse
    {

        public Resultheaderfield resultHeaderField { get; set; }
        public Querybalanceresultfield[] queryBalanceResultField { get; set; }

         

        public class Querybalanceresultfield
        {
            public string acctKeyField { get; set; }
            public Balanceresultfield[] balanceResultField { get; set; }
            public object outStandingListField { get; set; }
            public Accountcreditfield[] accountCreditField { get; set; }
        }

        public class Balanceresultfield
        {
            public string balanceTypeField { get; set; }
            public string balanceTypeNameField { get; set; }
            public int totalAmountField { get; set; }
            public string depositFlagField { get; set; }
            public string refundFlagField { get; set; }
            public string currencyIDField { get; set; }
            public Balancedetailfield[] balanceDetailField { get; set; }
        }

        public class Balancedetailfield
        {
            public long balanceInstanceIDField { get; set; }
            public int amountField { get; set; }
            public int initialAmountField { get; set; }
            public string effectiveTimeField { get; set; }
            public string expireTimeField { get; set; }
        }

        public class Accountcreditfield
        {
            public object creditLimitTypeField { get; set; }
            public object creditLimitTypeNameField { get; set; }
            public int totalCreditAmountField { get; set; }
            public int totalUsageAmountField { get; set; }
            public int totalRemainAmountField { get; set; }
            public string currencyIDField { get; set; }
            public object creditAmountInfoField { get; set; }
        }

    }
}
