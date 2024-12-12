using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblHotType
{
    [Key]
    public byte HotTypeId { get; set; }

    public required string HotType { get; set; }

    public byte? SplitCount { get; set; }

    public virtual ICollection<TblHotTypeCode> TblHotTypeCodes { get; set; } = new List<TblHotTypeCode>();
}
