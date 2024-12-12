using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class ConsoleAction
{
    [Key]
    public int Id { get; set; }

    public required string ActionName { get; set; }

    public virtual ICollection<ConsoleAccess> ConsoleAccesses { get; set; } = new List<ConsoleAccess>();
}
