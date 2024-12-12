using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblProductMetaData
{
    [Key]
    public int ProductMetaId { get; set; }

    public byte ProductId { get; set; }

    public byte ProductMetaDataTypeId { get; set; }

    public required string Data { get; set; }

    public virtual TblProduct Product { get; set; }

    public virtual TblProductMetaDataType ProductMetaDataType { get; set; }
}
