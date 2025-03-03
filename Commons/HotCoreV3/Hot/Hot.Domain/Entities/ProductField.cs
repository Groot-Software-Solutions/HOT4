namespace Hot.Domain.Entities;

public class ProductField
{
    public int ProductFieldId { get; set; }
    public int ProductId { get; set; }
    public string FieldName { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

}