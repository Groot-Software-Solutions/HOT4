using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblProductField
{
    [Key]
    public int BrandFieldId { get; set; }

    public byte ProductId { get; set; }

    public required string FieldName { get; set; }

    public required string DataType { get; set; }

    public required string Description { get; set; }

    public virtual TblProduct Product { get; set; }
}
