namespace Hot.Application.Common.Models;

public class UtilityAccountDetailsModel
{
    public string CustomerName { get; set; } = string.Empty;
    public string? Address { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public decimal Balance { get; set; } 
    public string Status { get; set; } = string.Empty;
    public decimal Arears { get; set; }
    public Currency? Currency { get; set; }
    public string Message { get; set; } = string.Empty;
}
