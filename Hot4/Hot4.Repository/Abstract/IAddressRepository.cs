using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IAddressRepository
    {
        Task<AddressModel?> GetAddress(long accountId);
        Task SaveUpdateAddress(Address address);
        Task DeleteAddress(Address address);
    }
}
