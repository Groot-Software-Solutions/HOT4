using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Application.Common.Models;

public record ProductModel(Product Product, List<ProductField> Fields, List<ProductMetaDataModel> MetaData);

public class ProductMetaDataModel : ProductMetaData
{
    public ProductMetaDataType MetaDataType { get; set; } = new();
}