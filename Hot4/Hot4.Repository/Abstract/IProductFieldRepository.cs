using Hot4.DataModel.Models;
using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface IProductFieldRepository
    {
        Task<List<ProductFieldModel>> ListProductField();
        Task AddProductField(ProductField productField);
        Task UpdateProductField(ProductField productField);
        Task DeleteProductField(ProductField productField);
    }
}
