using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IHotTypeRepository
    {
        Task<List<HotTypeModel>> ListHotType();
        Task<byte?> GetHotTypeIdentity(string typeCode, byte splitCount);
        Task AddHotType(HotTypes hotTypes);
        Task UpdateHotType(HotTypes hotTypes);
        Task DeleteHotType(HotTypes hotTypes);
    }
}
