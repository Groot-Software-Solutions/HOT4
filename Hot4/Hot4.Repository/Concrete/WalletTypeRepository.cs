using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class WalletTypeRepository : RepositoryBase<WalletType>, IWalletTypeRepository
    {
        public WalletTypeRepository(HotDbContext context) : base(context) { }
        public async Task AddWalletType(WalletType walletType)
        {
            await Create(walletType);
            await SaveChanges();
        }

        public async Task DeleteWalletType(WalletType walletType)
        {
            await Delete(walletType);
            await SaveChanges();
        }

        public async Task<WalletTypeModel?> GetWalletTypeById(int walletTypeId)
        {
            var result = await GetById(walletTypeId);
            if (result != null)
            {
                return new WalletTypeModel
                {
                    WalletTypeId = result.WalletTypeId,
                    WalletName = result.WalletName,
                };
            }
            return null;
        }

        public async Task<List<WalletTypeModel>> ListWalletType()
        {
            return await GetAll().Select(d =>
            new WalletTypeModel
            {
                WalletTypeId = d.WalletTypeId,
                WalletName = d.WalletName
            }).ToListAsync();
        }

        public async Task UpdateWalletType(WalletType walletType)
        {
            await Update(walletType);
            await SaveChanges();
        }
    }
}
