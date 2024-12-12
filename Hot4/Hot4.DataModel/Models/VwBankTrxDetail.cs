namespace Hot4.DataModel.Models;

public partial class VwBankTrxDetail
{
    public DateTime TrxDate { get; set; }

    public decimal Amount { get; set; }

    public required string RefName { get; set; }

    public decimal Balance { get; set; }

    public long? PaymentId { get; set; }

    public byte BankId { get; set; }

    public required string Bank { get; set; }

    public required string BankRef { get; set; }

    public required string Identifier { get; set; }

    public long BankTrxId { get; set; }
}
