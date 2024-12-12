using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblSelfTopUpState
{
    [Key]
    public byte SelfTopUpStateId { get; set; }

    public required string SelfTopUpStateName { get; set; }
}
