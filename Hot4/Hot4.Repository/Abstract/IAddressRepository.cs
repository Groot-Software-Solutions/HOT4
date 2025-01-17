using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IAddressRepository
    {
        Task<Address?> GetAddressById(long accountId);
        Task<bool> SaveAddress(Address address);
        Task<bool> UpdateAddress(Address address);
        Task<bool> DeleteAddress(Address address);
    }
}
