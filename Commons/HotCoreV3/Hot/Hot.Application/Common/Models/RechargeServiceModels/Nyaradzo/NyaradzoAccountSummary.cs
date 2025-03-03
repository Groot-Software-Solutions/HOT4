namespace Hot.Application.Common.Models.RechargeServiceModels.Nyaradzo;

public class NyaradzoAccountSummary
{
    public string Id { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; }
    public string PolicyNumber { get; set; } = string.Empty;
    public string SourceReference { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string ResponseCode { get; set; } = string.Empty;
    public string MonthlyPremium { get; set; } = string.Empty;
    public string AmountToBePaid { get; set; } = string.Empty;
    public string NumberOfMonths { get; set; } = string.Empty;
    public string ResponseDescription { get; set; } = string.Empty;
    public string Balance { get; set; } = string.Empty;
    public string PolicyHolder { get; set; } = string.Empty;
    public string BankCode { get; set; } = string.Empty;
    public string TxnRef { get; set; } = string.Empty;
    public Currency currency { get; set; }

}
