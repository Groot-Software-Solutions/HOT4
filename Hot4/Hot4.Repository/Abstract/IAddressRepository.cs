using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IAddressRepository
    {
        Task<Address?> GetAddress(long accountId);
        Task SaveUpdateAddress(Address address);
    }
}
