using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hot4.DataModel.Models;

public partial class HotTypeCode
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public byte HotTypeCodeId { get; set; }

    public byte HotTypeId { get; set; }

    public required string TypeCode { get; set; }

    public virtual HotTypes HotType { get; set; }
}
