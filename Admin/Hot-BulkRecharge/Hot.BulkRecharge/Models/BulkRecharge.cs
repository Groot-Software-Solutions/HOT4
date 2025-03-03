using Hot.API.Client;
using Hot.API.Client.Models;
using OneOf.Types;
using System;

namespace Hot.BulkRecharge
{
    public class BulkRecharge
    {

        public bool isValid { get; set; } = true;
        public string? LineData { get; set; } = null;
        public BulkRecharge(string mobile, string value, bool isData)
        {
            Mobile = mobile;
            Amount = isData ? 0 : Convert.ToDecimal(value);
            ProductCode = isData ? value : null;
        }

        public BulkRecharge(string mobile, string value, bool isData, bool isValid, string line)
        {
            Mobile = mobile;
            Amount = string.IsNullOrEmpty(value) ? 0 : isData ? 0 : Convert.ToDecimal(value);
            ProductCode = isData ? value : null;
            this.isValid = isValid;
            this.LineData = line;
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
        public void SetResult(RechargeResponse result)
        {
            Status = Enum.GetName(typeof(ReplyCode), result.ReplyCode) ?? "Unknown";
            Narrative = string.IsNullOrEmpty(result.ReplyMsg) ? result.ReplyMessage : result.ReplyMsg;
            ReplyCode = result.ReplyCode;
            Response = result;
        }
        public RechargeResponse? Response { get; set; }
        public bool IsUSD { get; set; } = false;
    }

}
