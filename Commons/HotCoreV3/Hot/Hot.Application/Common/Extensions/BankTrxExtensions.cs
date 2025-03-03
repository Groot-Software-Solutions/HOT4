namespace Hot.Application.Common.Extensions;
public static class BankTrxExtensions
{
    public static SMS GetSms(this BankTrx bankTrx, Access access, string smsText)
    {
        var sms = new SMS
        {
            Direction = false,
            InsertDate = DateTime.Now,
            Mobile = bankTrx.IsTransactionSentByAPI() ? bankTrx.Identifier : access.AccessCode,
            SMSID_In = null,
            SMSText = smsText,
            State = new State() { StateID = (int)States.Pending },
            Priority = new Priority() { PriorityId = (int)Priorities.Normal }
        };
        return sms;
    }
    public static bool IsTransactionSentByAPI(this BankTrx banktrx)
    {
        return banktrx.Branch.ToUpper().StartsWith("API");
    }
    public static bool IsAPITransaction(this BankTrx banktrx)
    {
        return banktrx.RefName.ToUpper().StartsWith("API");
    }
    public static bool IsZesaTransaction(this BankTrx banktrx)
    {
        return banktrx.RefName.ToUpper().StartsWith("ZESA");
    }
    public static bool IsSelfTopUpTransaction(this BankTrx banktrx)
    {
        return banktrx.RefName.ToUpper().StartsWith("SELF");
    }
    public static bool IsUSDTransaction(this BankTrx banktrx)
    {
        return banktrx.RefName.ToUpper().StartsWith("USD");
    }
    public static bool IsUSDUtilityTransaction(this BankTrx banktrx)
    {
        return banktrx.RefName.ToUpper().StartsWith("UUSD");
    }
    public static bool IsSelfTopUpTransaction(this BankTrx banktrx, IDbContext context, out SelfTopUp? selfTopUp)
    {
        selfTopUp = null;
        var selftTopUpResponse = context.SelfTopUps.GetByBankTrxIdAsync(banktrx.BankTrxID).Result;
        if (selftTopUpResponse.IsT1) return false;
        selfTopUp = selftTopUpResponse.AsT0;
        return true;
    }



    public static WalletTypes GetWalletType(this BankTrx bankTrx)
    {
        if (bankTrx.IsUSDTransaction()) return WalletTypes.USD;
        if (bankTrx.IsUSDUtilityTransaction()) return WalletTypes.USDUtility;
        if (bankTrx.IsZesaTransaction()) return WalletTypes.ZESA;
        return WalletTypes.ZWG;
    }
    public static string GetDealerSMS(this BankTrx banktrx, Account account, Payment payment, Template template)
    {
        template.SetAmount(banktrx.Amount)
            .SetBalance(account.WalletBalance(banktrx.GetWalletType()))
            .SetRefence(payment.Reference);
        return template.TemplateText;
    }
    public static string GetClientSMS(this BankTrx banktrx, string Reference, string Contact, decimal balance, Template template)
    {
        template.SetAmount(banktrx.Amount)
            .SetContact(Contact)
            .SetBalance(balance)
            .SetRefence(Reference);
        return template.TemplateText;
    }
    public static long GetAccessId(this BankTrx banktrx)
    {
        var parsed = long.TryParse(banktrx.Branch[(banktrx.Branch.LastIndexOf('-') + 1)..], out long result);
        return parsed ? result : 0;
    }
    public static bool HasTransactionBeenProcessed(this BankTrx banktrx)
    {
        return banktrx.BankTrxStateID switch
        {
            (byte)BankTransactionStates.BusyConfirming => false,
            (byte)BankTransactionStates.ToBeAllocated => false,
            (byte)BankTransactionStates.Pending => false,
            (byte)BankTransactionStates.Suspended => false,
            _ => true
        };
    }

    public static PaymentTypes GetPaymentTypeId(this BankTrx banktrx)
    { 
        if (banktrx.IsZesaTransaction()) return PaymentTypes.ZESA;
        if (banktrx.IsUSDTransaction()) return PaymentTypes.USD;
        if (banktrx.IsUSDUtilityTransaction()) return PaymentTypes.USDUtility;
        return PaymentTypes.BankAuto;

    }
}
