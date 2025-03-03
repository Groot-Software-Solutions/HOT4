namespace Hot.Application.Common.Models
{
    public class CompleteBankTrxResult
    {
        public CompleteBankTrxResult()
        {
        }

        public CompleteBankTrxResult(bool successful, string errorData)
        {
            Successful = successful;
            ErrorData = errorData;
        }

        public CompleteBankTrxResult(bool successful, BankTrx? bankTrx, Payment? payment)
        {
            Successful = successful;
            BankTrx = bankTrx;
            Payment = payment;
        }

        public bool Successful { get; set; } = false;
        public string? ErrorData { get; set; } = null;
        public BankTrx? BankTrx { get; set; } = null;
        public Payment? Payment { get; set; } = null;
    }
}
