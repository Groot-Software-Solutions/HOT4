namespace Hot.Application.Common.Models;

public class MobileAccountDetailsModel
{
    public string CustomerName { get; set; } = string.Empty; 
    public string AccountNumber { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal Arears { get; set; } 
}
