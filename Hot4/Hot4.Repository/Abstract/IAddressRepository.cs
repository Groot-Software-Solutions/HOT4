using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IAddressRepository
    {
        Task<Address?> GetAddressById(long accountId);
        Task SaveAddress(Address address);
        Task UpdateAddress(Address address);
        Task DeleteAddress(Address address);
    }
}
