
namespace Hot.Application.Actions.ProductActions;

public record GetProductsQuery : IRequest<OneOf<List<ProductModel>, AppException>>;

public class GetProductQueryHandler : IRequestHandler<GetProductsQuery, OneOf<List<ProductModel>, AppException>>
{
    private readonly ILogger<GetProductQueryHandler> logger;
    private readonly IDbContext dbContext;

    public GetProductQueryHandler(ILogger<GetProductQueryHandler> logger, IDbContext dbContext)
    {
        this.logger = logger;
        this.dbContext = dbContext;
    }

    public async Task<OneOf<List<ProductModel>, AppException>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var productResponse = await dbContext.Products.ListAsync();
            if (productResponse.IsT1) return productResponse.AsT1.LogAndReturnError(logger);
            var products = productResponse.AsT0;

            var productFieldsResponse = await dbContext.ProductFields.ListAsync();
            if (productFieldsResponse.IsT1) return productFieldsResponse.AsT1.LogAndReturnError(logger);
            var productFileds = productFieldsResponse.AsT0;

            var productMetaDataResponse = await dbContext.ProductMetaDatas.ListAsync();
            if (productMetaDataResponse.IsT1) return productMetaDataResponse.AsT1.LogAndReturnError(logger);
            var productMetaData = productMetaDataResponse.AsT0;

            var productMetaDataTypeResponse = await dbContext.ProductMetaDataTypes.ListAsync();
            if (productMetaDataTypeResponse.IsT1) return productMetaDataTypeResponse.AsT1.LogAndReturnError(logger);
            var productMetaDataTypes = productMetaDataTypeResponse.AsT0;

            var result = MapToProductModelList(products, productFileds, productMetaData, productMetaDataTypes);

            return result;
        }
        catch (Exception ex)
        {
            return ex.LogAndReturnError(logger);
        }
    }

    private static List<ProductModel> MapToProductModelList(List<Product> products, List<ProductField> productFileds, List<ProductMetaData> productMetaData, List<ProductMetaDataType> productMetaDataTypes)
    {
        List<ProductModel> result = new();
        foreach (var product in products.Where(p=>p.ProductStateId == ProductStates.Active))
        {
            var fields = productFileds.Where(b => b.ProductId == product.ProductId).ToList();
            var meta = productMetaData.Where(b => b.ProductId == product.ProductId).Select(b => MapToModel(productMetaDataTypes, b)).ToList();
            result.Add(new ProductModel(product, fields, meta));
        } 
        return result;
    }

    private static ProductMetaDataModel MapToModel(List<ProductMetaDataType> productMetaDataTypes, ProductMetaData b)
    {
        return new ProductMetaDataModel()
        {
            ProductMetaId = b.ProductMetaId,
            Data = b.Data,
            ProductId = b.ProductId,
            ProductMetaDataTypeId = b.ProductMetaDataTypeId,
            MetaDataType = productMetaDataTypes.First(m => m.ProductMetaDataTypeId == b.ProductMetaDataTypeId)
        };
    }
}


