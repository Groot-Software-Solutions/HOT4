namespace Hot.Ecocash.Lesotho.Models;

public class QueryTransactionRequestLesothoModel
{
    public string transactionId { get; set; } = string.Empty;
    public string vendor_code { get; set; } = "Hotrecharge";
    public string api_key { get; set; } = "";   
    public string checksum { get; set; } = "";  
}
