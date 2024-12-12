using System.ComponentModel;

namespace Hot4.Core.DataViewModels
{
    public class AccountAccessModel
    {
        [DisplayName("Access Id")]
        public long AccessID { get; set; }

        public string Channel { get; set; }
        public long ChannelID { get; set; }

        [DisplayName("User/Device Id")]
        public string AccessCode { get; set; }

        [DisplayName("Disabled")]
        public bool Deleted { get; set; }

        public long AccountID { get; set; }
    }
}
