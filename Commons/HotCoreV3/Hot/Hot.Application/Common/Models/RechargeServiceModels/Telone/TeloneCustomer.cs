namespace Hot.Application.Common.Models.RechargeServiceModels.Telone;

public class TeloneCustomerResult
{
    public TeloneCustomer Account { get; set; } 
    public bool Successful { get; set; }
    public string TransactionResult { get; set; } = string.Empty;
    public string RawResponseData { get; set; } = string.Empty;
}
public class TeloneCustomer
{
    public string AccountName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string AccountAddress { get; set; } = string.Empty;
    public string ResponseDescription { get; set; } = string.Empty;
}