using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class ProductMetaData
{
    [Key]
    public int ProductMetaId { get; set; }

    public byte ProductId { get; set; }

    public byte ProductMetaDataTypeId { get; set; }

    public required string Data { get; set; }

    public virtual Product Product { get; set; }

    public virtual ProductMetaDataType ProductMetaDataType { get; set; }
}
