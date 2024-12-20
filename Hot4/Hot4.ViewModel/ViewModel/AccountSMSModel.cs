using System.ComponentModel;

namespace Hot4.Core.DataViewModels
{
    public class AccountSmsModel
    {
        [DisplayName("SMS Id")]
        public long SMSID { get; set; }

        [DisplayName("Smmp Id")]
        public byte SmppID { get; set; }

        [DisplayName("State")]
        public string State { get; set; }

        [DisplayName("Priority")]
        public string Priority { get; set; }

        [DisplayName("Direction")]
        public string DrectionText { get; set; }

        [DisplayName("Mobile")]
        public string Mobile { get; set; }

        [DisplayName("SMS Text")]
        public string SMSText { get; set; }

        [DisplayName("SMS Date")]
        public DateTime SMSDate { get; set; }

        [DisplayName("SMSID In")]
        public long? SMSID_In { get; set; }

        [DisplayName("Insert Date")]
        public DateTime? InsertDate { get; set; }
    }
}
