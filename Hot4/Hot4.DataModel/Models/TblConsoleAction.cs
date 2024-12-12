using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblConsoleAction
{
    [Key]
    public int Id { get; set; }

    public required string ActionName { get; set; }

    public virtual ICollection<TblConsoleAccess> TblConsoleAccesses { get; set; } = new List<TblConsoleAccess>();
}
