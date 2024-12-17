using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class ProductMetaDataType
{
    [Key]
    public byte ProductMetaDataTypeId { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public virtual ICollection<ProductMetaData> ProductMetaDatas { get; set; } = new List<ProductMetaData>();
}
