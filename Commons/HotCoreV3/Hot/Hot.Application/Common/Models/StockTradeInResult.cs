namespace Hot.Application.Common.Models;
public class StockTradeInResult
{
    public int Result { get; set; }
    public string Message { get; set; }
    public decimal ZWLBalance { get; set; }
    public decimal USDBalance { get; set; }
}
