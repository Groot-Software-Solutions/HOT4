namespace Hot4.ViewModel
{
    public class AccountAccessModel
    {
        public long AccessId { get; set; }
        public long AccountId { get; set; }
        public string Channel { get; set; }
        public long ChannelId { get; set; }
        public string AccessCode { get; set; }
        public string AccessPassword { get; set; }
        public bool Deleted { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

    }
}
