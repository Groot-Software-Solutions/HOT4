namespace Hot.Ecocash.Lesotho.Models;
public class PayMerchantRequestLesothoModel
{
    public string msisdn { get; set; } = string.Empty;
    public string merchantNumber { get; set; } = string.Empty;
    public string merchCode { get; set; } = string.Empty;
    public string amount { get; set; } = string.Empty;
    public string requestId { get; set; } = string.Empty;
    public string vendor_code { get; set; } = "2020";
    public string api_key { get; set; } = "";
    public string checksum { get; set; } = "";
    public string callbackurl { get; set; } = "";

}
