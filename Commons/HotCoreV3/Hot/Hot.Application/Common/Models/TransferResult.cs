namespace Hot.Application.Common.Models
{
    public class TransferResult
    {
        public int TransferId { get; set; }
        public decimal Amount { get; set; }
        public string TargetAccountName { get; set; } = string.Empty;
        public string SourceAccountName { get; set; } = string.Empty;
        public long ToAccountId { get; set; }
        public long FromAccountId { get; set; }
        public decimal WalletBalance { get; set; }
        public ReplyCode ReplyCode { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
