namespace Hot.Application.Common.Models
{
    public class SelfTopUpResult
    {
        public SelfTopUpResult()
        {
        }

        public SelfTopUpResult(bool successful, Recharge? recharge)
        {
            Successful = successful;
            Recharge = recharge;
        }

        public SelfTopUpResult(bool successful, BankTrx? bankTrx, SelfTopUp selfTopUp)
        {
            Successful = successful;
            BankTrx = bankTrx;

        }
        public SelfTopUpResult(bool successful, string? errorData, BankTrx? bankTrx)
        {
            Successful = successful;
            ErrorData = errorData;
            BankTrx = bankTrx;
        }
        public SelfTopUpResult(bool successful, string? errorData, Recharge? recharge)
        {
            Successful = successful;
            ErrorData = errorData;
            Recharge = recharge;
        }

        public bool Successful { get; set; } = false;
        public Recharge? Recharge { get; set; } = null;
        public BankTrx? BankTrx { get; set; } = null;
        public string? ErrorData { get; set; } = null;
        public SelfTopUp? SelfTopUp { get; set; } = null;
    }
}
