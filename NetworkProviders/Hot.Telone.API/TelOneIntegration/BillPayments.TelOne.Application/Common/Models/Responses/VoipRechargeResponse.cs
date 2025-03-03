using System;
using System.Collections.Generic;
using System.Text;

namespace TelOne.Application.Common.Models
{
    public class VoipRechargeResponse
    { 
        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }
        public string AccountNumber { get; set; }
        public string MerchantReference { get; set; }
        public string OrderNumer { get; set; }
        public Rechargeresult RechargeResult { get; set; }
        public decimal Balance { get; set; }

        public class Rechargeresult
        {
            public Resultheaderfield resultHeaderField { get; set; }
            public Rechargeresultfield rechargeResultField { get; set; }
        }
  
        public class Rechargeresultfield
        {
            public string rechargeSerialNoField { get; set; }
            public Balancechginfofield[] balanceChgInfoField { get; set; }
            public Loanchginfofield loanChgInfoField { get; set; }
            public Rechargebonusfield rechargeBonusField { get; set; }
            public Lifecyclechginfofield lifeCycleChgInfoField { get; set; }
            public Creditchginfofield[] creditChgInfoField { get; set; }
            public string faceValueField { get; set; }
        }

        public class Loanchginfofield
        {
            public int oldLoanAmtField { get; set; }
            public int newLoanAmtField { get; set; }
            public int loanPaymentAmtField { get; set; }
            public int loanInterestAmtField { get; set; }
            public string currencyIDField { get; set; }
        }

        public class Rechargebonusfield
        {
            public Freeunititemlistfield[] freeUnitItemListField { get; set; }
            public Balancelistfield[] balanceListField { get; set; }
        }

        public class Freeunititemlistfield
        {
            public string freeUnitIDField { get; set; }
            public string freeUnitTypeField { get; set; }
            public string freeUnitTypeNameField { get; set; }
            public string measureUnitField { get; set; }
            public string measureUnitNameField { get; set; }
            public int bonusAmtField { get; set; }
            public string effectiveTimeField { get; set; }
            public string expireTimeField { get; set; }
        }

        public class Balancelistfield
        {
            public string balanceTypeField { get; set; }
            public string balanceIDField { get; set; }
            public string balanceTypeNameField { get; set; }
            public int bonusAmtField { get; set; }
            public string currencyIDField { get; set; }
            public string effectiveTimeField { get; set; }
            public string expireTimeField { get; set; }
        }

        public class Lifecyclechginfofield
        {
            public Oldlifecyclestatusfield[] oldLifeCycleStatusField { get; set; }
            public Newlifecyclestatusfield[] newLifeCycleStatusField { get; set; }
            public string addValidityField { get; set; }
        }

        public class Oldlifecyclestatusfield
        {
            public string statusNameField { get; set; }
            public string statusExpireTimeField { get; set; }
            public string statusIndexField { get; set; }
        }

        public class Newlifecyclestatusfield
        {
            public string statusNameField { get; set; }
            public string statusExpireTimeField { get; set; }
            public string statusIndexField { get; set; }
        }

        public class Balancechginfofield
        {
            public string balanceTypeField { get; set; }
            public int balanceIDField { get; set; }
            public bool balanceIDFieldSpecified { get; set; }
            public string balanceTypeNameField { get; set; }
            public int oldBalanceAmtField { get; set; }
            public int newBalanceAmtField { get; set; }
            public string currencyIDField { get; set; }
        }

        public class Creditchginfofield
        {
            public int creditLimitIDField { get; set; }
            public bool creditLimitIDFieldSpecified { get; set; }
            public string creditLimitTypeField { get; set; }
            public string creditLimitTypeNameField { get; set; }
            public int oldLeftCreditAmtField { get; set; }
            public int newLeftCreditAmtField { get; set; }
            public string measureUnitField { get; set; }
        }

    }
}
