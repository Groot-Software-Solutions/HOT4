using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hot4.DataModel.Models;

public partial class ConsoleAccess
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required string RoleName { get; set; }

    public int ConsoleActionId { get; set; }

    public virtual ConsoleAction ConsoleAction { get; set; }
}
