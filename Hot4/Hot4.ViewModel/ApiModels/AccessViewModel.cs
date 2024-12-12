namespace Hot4.ViewModel.ApiModels
{
    public class AccessViewModel
    {
        public long AccessId { get; set; }

        public long AccountId { get; set; }

        public byte ChannelId { get; set; }

        public string AccessCode { get; set; } = null!;

        public string AccessPassword { get; set; } = null!;

        public bool? Deleted { get; set; }

        public string? PasswordHash { get; set; }

        public string? PasswordSalt { get; set; }

        public DateTime? InsertDate { get; set; }
    }
}
