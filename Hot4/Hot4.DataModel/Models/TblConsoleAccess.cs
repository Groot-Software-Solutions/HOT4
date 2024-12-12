using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblConsoleAccess
{
    [Key]
    public required string RoleName { get; set; }

    public int ConsoleActionId { get; set; }

    public virtual TblConsoleAction ConsoleAction { get; set; }
}
