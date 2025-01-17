using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface IAddressService
    {

        Task<AddressModel?> GetAddressById(long accountId);
        Task<bool> SaveAddress(AddressModel addressModel);
        Task<bool> UpdateAddress(AddressModel addressModeless);
        Task<bool> DeleteAddress(AddressModel addressModel);

    }
}
