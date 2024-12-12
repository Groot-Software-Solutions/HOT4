namespace Hot4.DataModel.Models;

public partial class VwBankTrxBatch
{
    public long BankTrxBatchId { get; set; }

    public byte BankId { get; set; }

    public required string Bank { get; set; }

    public DateTime BatchDate { get; set; }

    public required string BatchReference { get; set; }

    public required string LastUser { get; set; }
}
