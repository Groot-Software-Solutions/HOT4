using Hot.API.Client;
using System;

namespace App
{
    public class BulkRecharge
    {
        public BulkRecharge(string mobile, string value, bool isData)
        {
            Mobile = mobile;
            Amount = isData ? 0 : Convert.ToDecimal(value);
            ProductCode = isData ? value : null; 
        }
        public int Id { get; set; }
        public string Reference { get; set; } = Guid.NewGuid().ToString();
        public string Mobile { get; set; }
        public decimal Amount { get; set; }
        public string ProductCode { get; set; }
        public string CustomSMS { get; set; }
        public string Status { get; set; } = "Pending";
        public string Narrative { get; set; }
        public int ReplyCode { get; set; }

        public bool HasCustomSMS => !string.IsNullOrEmpty(CustomSMS);
        public bool IsData => !string.IsNullOrEmpty(ProductCode);
        public void SetResult(int replyCode,string narrative)
        {
            Status = Enum.GetName(typeof(ReplyCode), replyCode);
            Narrative = narrative;
            ReplyCode = replyCode;
        }

    }

}
