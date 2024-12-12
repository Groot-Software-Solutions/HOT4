using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblHotTypeCode
{
    [Key]
    public byte HotTypeCodeId { get; set; }

    public byte HotTypeId { get; set; }

    public required string TypeCode { get; set; }

    public virtual TblHotType HotType { get; set; }
}
