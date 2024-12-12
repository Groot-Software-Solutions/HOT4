using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblProductMetaDataType
{
    [Key]
    public byte ProductMetaDataTypeId { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public virtual ICollection<TblProductMetaData> TblProductMetaData { get; set; } = new List<TblProductMetaData>();
}
