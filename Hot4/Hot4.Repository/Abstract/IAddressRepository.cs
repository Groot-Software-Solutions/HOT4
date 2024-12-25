using Hot4.DataModel.Models;
using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface IAddressRepository
    {
        Task<AddressModel?> GetAddress(long accountId);
        Task SaveUpdateAddress(Address address);
    }
}
