using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hot4.DataModel.Models;

public partial class SelfTopUpState
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public byte SelfTopUpStateId { get; set; }

    public required string SelfTopUpStateName { get; set; }
}
