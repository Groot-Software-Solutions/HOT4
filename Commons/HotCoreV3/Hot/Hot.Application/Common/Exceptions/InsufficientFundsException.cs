namespace Hot.Application.Common.Exceptions;

public class InsufficientFundsException : Exception
{
    public InsufficientFundsException(string name, object data)
      : base($"Insufficient Funds Exception \"{name}\" {data}")
    {
    }
    public InsufficientFundsException(string data, Exception error)
  : base($"Insufficient Funds Exception - \"{data}\" - {error.Message}", error)
    {
    }

    public InsufficientFundsException(decimal balance, decimal cost, decimal saleValue, string mobile, IDbContext dbContext)
    {
        Balance = balance;
        Cost = cost;
        SaleValue = saleValue;
        var template = TemplateExtensions.GetTemplate(dbContext, Templates.FailedRechargeBalance);
        template?.SetBalance(Balance).SetSaleValue(SaleValue).SetCost(Cost).SetMobile(mobile);
        ResponseMessage = template?.TemplateText ?? "";
    }

    public decimal Balance { get; set; }
    public decimal Cost { get; set; }
    public decimal SaleValue { get; set; }
    public string? ResponseMessage { get; set; }

}
