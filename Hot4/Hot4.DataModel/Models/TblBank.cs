using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblBank
{
    [Key]
    public byte BankId { get; set; }

    public required string Bank { get; set; }

    public int? SageBankId { get; set; }

    public virtual ICollection<TblBankTrxBatch> TblBankTrxBatches { get; set; } = new List<TblBankTrxBatch>();
}
