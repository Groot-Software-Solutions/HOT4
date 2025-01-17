using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;

namespace Hot4.Repository.Concrete
{
    public class AddressRepository : RepositoryBase<Address>, IAddressRepository
    {
        public AddressRepository(HotDbContext context) : base(context) { }
        public async Task<Address?> GetAddressById(long accountId)
        {
            var record = await GetById(accountId);
            if (record != null)
            {
                return record;
            }

            return null;
        }
        public async Task<bool> SaveAddress(Address address)
        {
            address.InvoiceFreq = 1;
            await Create(address);
            await SaveChanges();
            return true;
        }
        public async Task<bool> UpdateAddress(Address address)
        {
            Update(address);
            await SaveChanges();
            return true;
        }
        public async Task<bool> DeleteAddress(Address address)
        {
            Delete(address);
            await SaveChanges();
            return true;
        }
    }
}
