using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class SelfTopUpState
{
    [Key]
    public byte SelfTopUpStateId { get; set; }

    public required string SelfTopUpStateName { get; set; }
}
