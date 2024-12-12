using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class ConsoleAccess
{
    [Key]
    public required string RoleName { get; set; }

    public int ConsoleActionId { get; set; }

    public virtual ConsoleAction ConsoleAction { get; set; }
}
