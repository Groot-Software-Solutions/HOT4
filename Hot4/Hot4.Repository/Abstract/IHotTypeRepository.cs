using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IHotTypeRepository
    {
        Task<TblHotType?> GetHotType(int hotTypeId);
        Task<List<TblHotType>> ListHotType();
    }
}
