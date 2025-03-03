namespace Hot.Application.Common.Interfaces.DbContextTables;

public interface IProductMetaDataTypes : IDbContextTable<ProductMetaDataType>
    , IDbCanList<ProductMetaDataType>

{ }