using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IHotTypeRepository
    {
        Task<HotTypes?> GetHotType(int hotTypeId);
        Task<List<HotTypes>> ListHotType();
    }
}
