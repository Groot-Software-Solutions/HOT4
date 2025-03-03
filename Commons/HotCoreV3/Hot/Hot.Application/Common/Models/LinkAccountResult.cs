namespace Hot.Application.Common.Models
{
    public class LinkAccountResult
    {
        public string AccountName { get; set; } = string.Empty;
        public long AccountId { get; set; }
        public bool Success { get; set; }
        public string AccessCode { get; set; } = string.Empty;
    }
}
