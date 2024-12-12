using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class HotTypes
{
    [Key]
    public byte HotTypeId { get; set; }

    public required string HotType { get; set; }

    public byte? SplitCount { get; set; }

    public virtual ICollection<HotTypeCode> HotTypeCodes { get; set; } = new List<HotTypeCode>();
}
