using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblLimitType
{
    [Key]
    public int LimitTypeId { get; set; }

    public required string LimitTypeName { get; set; }
}
