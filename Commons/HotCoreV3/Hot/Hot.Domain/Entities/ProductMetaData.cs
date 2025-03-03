namespace Hot.Domain.Entities;

public class ProductMetaData
{
    public int ProductMetaId { get; set; }
    public int ProductId { get; set; } 
    public int ProductMetaDataTypeId { get; set; }  
    public string Data { get; set; } = string.Empty;

}
