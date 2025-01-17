using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class WalletTypeRepository : RepositoryBase<WalletType>, IWalletTypeRepository
    {
        public WalletTypeRepository(HotDbContext context) : base(context) { }
        public async Task<bool> AddWalletType(WalletType walletType)
        {
            await Create(walletType);
            await SaveChanges();
            return true;
        }

        public async Task<bool> DeleteWalletType(WalletType walletType)
        {
            Delete(walletType);
            await SaveChanges();
            return true;
        }

        public async Task<WalletType?> GetWalletTypeById(int walletTypeId)
        {
            return await GetById(walletTypeId);
        }

        public async Task<List<WalletType>> ListWalletType()
        {
            return await GetAll().ToListAsync();
        }

        public async Task<bool> UpdateWalletType(WalletType walletType)
        {
            Update(walletType);
            await SaveChanges();
            return true;
        }
    }
}
