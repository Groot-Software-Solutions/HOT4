using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IHotTypeRepository
    {
        Task<HotTypes?> GetHotTypeById(byte HotTypeId);
        Task<List<HotTypes>> ListHotType();
        Task<byte?> GetHotTypeIdentity(string typeCode, byte splitCount);
        Task<bool> AddHotType(HotTypes hotTypes);
        Task<bool> UpdateHotType(HotTypes hotTypes);
        Task<bool> DeleteHotType(HotTypes hotTypes);
    }
}
