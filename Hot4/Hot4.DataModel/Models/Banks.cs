using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class Banks
{
    [Key]
    public byte BankId { get; set; }

    public required string Bank { get; set; }

    public int? SageBankId { get; set; }

    public virtual ICollection<BankTrxBatch> BankTrxBatches { get; set; } = new List<BankTrxBatch>();
}
