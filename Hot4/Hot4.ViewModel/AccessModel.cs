namespace Hot4.ViewModel
{
    public class AccessModel
    {
        public long AccessId { get; set; }
        public long AccountId { get; set; }
        public byte ChannelId { get; set; }
        public required string AccessCode { get; set; }
        public required string AccessPassword { get; set; }
        public bool? Deleted { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
        public DateTime? InsertDate { get; set; }
    }
}
