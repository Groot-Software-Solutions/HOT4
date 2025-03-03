namespace Hot.Domain.Entities
{
    public class Access
    {
        public long AccessId { get; set; }

        public long AccountId { get; set; }

        public byte ChannelID { get; set; }

        public string AccessCode { get; set; } = string.Empty;

        public string AccessPassword { get; set; } = string.Empty;

        public bool? Deleted { get; set; }

        public string? PasswordHash { get; set; }

        public string? PasswordSalt { get; set; }

        public DateTime? InsertDate { get; set; } 
    }
}
