using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hot4.DataModel.Models;

public partial class ConsoleAction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    public required string ActionName { get; set; }

    public virtual ICollection<ConsoleAccess> ConsoleAccesses { get; set; } = new List<ConsoleAccess>();
}
