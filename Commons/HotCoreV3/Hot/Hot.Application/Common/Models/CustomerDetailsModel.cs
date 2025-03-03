namespace Hot.Application.Common.Models;
public class CustomerDetailsModel
{
public string AccountNumber { get; set; } = string.Empty;
public string AccountName { get; set; } = string.Empty;
public Currency? Currency { get; set; }
public string Address { get; set; } = string.Empty;
public decimal Balance { get; set; }
public decimal Arrears { get; set; }
public string Status { get; set; } = string.Empty;
    public string Message { get; internal set; }
}