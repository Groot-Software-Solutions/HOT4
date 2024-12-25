using Hot4.DataModel.Models;
using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface IConfigRepository
    {
        Task<List<ConfigModel>> GetConfig();
        Task AddConfig(Configs config);
        Task UpdateConfig(Configs config);
        Task DeleteConfig(Configs config);
    }
}
