namespace Hot4.DataModel.Models;

public partial class VwBankTrx
{
    public long BankTrxId { get; set; }

    public long BankTrxBatchId { get; set; }

    public byte BankTrxTypeId { get; set; }

    public required string BankTrxType { get; set; }

    public byte BankTrxStateId { get; set; }

    public required string BankTrxState { get; set; }

    public decimal Amount { get; set; }

    public DateTime TrxDate { get; set; }

    public required string Identifier { get; set; }

    public required string RefName { get; set; }

    public required string Branch { get; set; }

    public required string BankRef { get; set; }

    public decimal Balance { get; set; }

    public long? PaymentId { get; set; }
}
