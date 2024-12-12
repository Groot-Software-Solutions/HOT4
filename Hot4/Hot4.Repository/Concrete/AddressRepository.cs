using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;

namespace Hot4.Repository.Concrete
{
    public class AddressRepository : RepositoryBase<Address>, IAddressRepository
    {
        public AddressRepository(HotDbContext context) : base(context) { }
        public async Task<Address?> GetAddress(long accountId)
        {
            return await GetById(accountId);
        }

        public async Task InsertAddress(Address address)
        {
            await Create(address);
            await SaveChanges();
        }
    }
}
