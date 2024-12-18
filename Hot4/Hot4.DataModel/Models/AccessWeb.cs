using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hot4.DataModel.Models;

public partial class AccessWeb
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long AccessId { get; set; }
    public required string AccessName { get; set; }
    public required string WebBackground { get; set; }
    public bool SalesPassword { get; set; }
    public string? ResetToken { get; set; }
}
