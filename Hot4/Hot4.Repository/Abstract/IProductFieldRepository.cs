using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IProductFieldRepository
    {
        Task<ProductField> GetProductFieldById(int BrandFieldId);
        Task<List<ProductFieldModel>> ListProductField();
        Task <bool>AddProductField(ProductField productField);
        Task <bool>UpdateProductField(ProductField productField);
        Task <bool>DeleteProductField(ProductField productField);
    }
}
