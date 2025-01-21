using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface IHotTypeService
    {
        Task<HotTypeModel> GetHotTypeById(byte hotTypeId);
        Task<List<HotTypeModel>> ListHotType();
        Task<byte?> GetHotTypeIdentity(string typeCode, byte splitCount);
        Task<bool> AddHotType(HotTypeRecord hotTypeModel);
        Task<bool> UpdateHotType(HotTypeRecord hotTypeModel);
        Task<bool> DeleteHotType(byte hotTypeId);
    }
}
