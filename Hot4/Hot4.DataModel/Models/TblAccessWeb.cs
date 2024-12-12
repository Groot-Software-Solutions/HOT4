using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblAccessWeb
{
    [Key]
    public long AccessId { get; set; }

    public required string AccessName { get; set; }

    public required string WebBackground { get; set; }

    public bool SalesPassword { get; set; }

    public string? ResetToken { get; set; }
}
