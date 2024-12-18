namespace Hot4.Core.DataViewModels
{
    public class AccountAccessModel
    {
        public long AccessID { get; set; }
        public long AccountID { get; set; }
        public string Channel { get; set; }
        public long ChannelID { get; set; }
        public string AccessCode { get; set; }
        public string AccessPassword { get; set; }
        public bool Deleted { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

    }
}
