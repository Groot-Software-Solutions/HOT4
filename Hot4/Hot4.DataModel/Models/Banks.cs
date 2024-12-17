using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hot4.DataModel.Models;

public partial class Banks
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public byte BankId { get; set; }

    public required string Bank { get; set; }

    public int? SageBankId { get; set; }

    public virtual ICollection<BankTrxBatch> BankTrxBatches { get; set; } = new List<BankTrxBatch>();
}
