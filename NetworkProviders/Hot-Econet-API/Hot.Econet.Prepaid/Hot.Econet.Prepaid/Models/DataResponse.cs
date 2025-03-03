namespace Hot.Econet.Prepaid.Models;

public class DataResponse
{
    public string Description { get; set; } = string.Empty;
    public string ProviderReference { get; set; } = string.Empty;
    public string Serial { get; set; } = string.Empty;
    public int Status { get; set; }
    public int StatusCode { get; set; }
    public string RawResponseData { get; set; } = string.Empty;
    public decimal InitialWalletBalance { get; set; }
    public decimal FinalWalletBalance { get; set; } 
}




