using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hot4.DataModel.Models;

public partial class PinBatchTypes
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public byte PinBatchTypeId { get; set; }

    public required string PinBatchType { get; set; }

    public virtual ICollection<PinBatches> PinBatches { get; set; } = new List<PinBatches>();
}
