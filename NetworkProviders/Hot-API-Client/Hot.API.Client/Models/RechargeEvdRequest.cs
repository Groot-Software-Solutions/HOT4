namespace Hot.API.Client.Models
{
    public class RechargeEvdRequest:BulkEvdRequest
    {
        public string TargetNumber { get; set; }
        public string Reference { get; internal set; }
    }
}
