using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IAddressRepository
    {
        Task<TblAddress?> GetAddress(long accountId);

        Task InsertAddress(TblAddress address);
    }
}
