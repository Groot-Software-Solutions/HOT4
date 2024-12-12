namespace Hot4.DataModel.Models;

public partial class ZvwProductlistdetail
{
    public byte BrandId { get; set; }

    public required string BrandName { get; set; }

    public required string Name { get; set; }

    public int BrandFieldId { get; set; }

    public required string FieldName { get; set; }

    public required string DataType { get; set; }

    public byte ProductMetaDataTypeId { get; set; }

    public required string Data { get; set; }

    public required string Expr1 { get; set; }

    public required string Description { get; set; }

    public required string WalletName { get; set; }

    public int WalletTypeId { get; set; }
}
