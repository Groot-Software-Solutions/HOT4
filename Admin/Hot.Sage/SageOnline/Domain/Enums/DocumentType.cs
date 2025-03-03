namespace Sage.Domain.Enums
{
    public enum DocumentType : int
    {
        CustomerOpeningBalance = 0,
        Quote = 1,
        TaxInvoice = 2,
        CustomerReturn = 3,
        Receipt = 4,
        WriteOff = 5,
        CustomerDiscount = 6,
        RecurringTaxInvoice = 7,
        DraftTaxInvoice = 8,
        CustomerJournal = 9,
        DeliveryNote = 52,
        SupplierOpeningBalance = 100,
        PurchaseOrder = 101,
        SupplierInvoice = 102,
        SupplierReturn = 103,
        Payment = 104,
        SupplierDiscount = 106,
        SupplierJournal = 109,
        ItemOpeningBalance = 200,
        ItemAdjustment = 201,
        AccountOpeningBalance = 300,
        CashbookReceipt = 301,
        CashbookPayment = 302,
        JournalEntry = 303,
        BankAccountOpeningBalance = 400,
        BankAccountTransferFrom = 401,
        BankAccountTransferTo = 402,
        SplitCashbookPayment = 403,
        SplitCashbookReceipt = 404,
        SplitPayment = 405,
        SplitReceipt = 406,
        TaxPayment = 501,
        TaxRefund = 502,
        InputTaxAdjustment = 503,
        OutputTaxAdjustment = 504,
        InputTaxProvisionAdjustment = 505,
        OutputTaxProvisionAdjustment = 50
    }



}
