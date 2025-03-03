namespace Hot.Application.Common.Models;
public class RechargeResult
{
    public bool Successful { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? ErrorData { get; set; }
    public object? Data { get; set; }
    public Recharge? Recharge { get; set; }
    public RechargePrepaid? RechargePrepaid { get; set; }
    public decimal? WalletBalance { get; set; }

}

