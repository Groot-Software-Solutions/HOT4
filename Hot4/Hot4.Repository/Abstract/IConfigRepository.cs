using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IConfigRepository
    {
        Task<Configs> GetConfigById(byte ConfigId);
        Task<List<Configs>> ListConfig();
        Task<bool> AddConfig(Configs config);
        Task<bool> UpdateConfig(Configs config);
        Task<bool> DeleteConfig(Configs config);
    }
}
