using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface IProductFieldRepository
    {
        Task<List<ProductFieldModel>> ListProductField();
    }
}
