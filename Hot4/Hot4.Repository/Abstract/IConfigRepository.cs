using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface IConfigRepository
    {
        Task<List<ConfigModel>> GetConfig();
    }
}
