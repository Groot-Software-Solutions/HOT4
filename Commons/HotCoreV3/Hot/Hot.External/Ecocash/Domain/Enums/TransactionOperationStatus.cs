namespace Hot.Ecocash.Domain.Enums
{
    public enum TransactionOperationStatus : int
    {
        Charged = 1,
        Refunded = 2,
        Refund = 7,
        ListTransaction = 3,
        Processing = 4,
        Denied = 5,
        Refused = 6,
         
    }
}
