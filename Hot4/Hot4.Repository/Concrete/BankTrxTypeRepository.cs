using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class BankTrxTypeRepository : RepositoryBase<BankTrxTypes>, IBankTrxTypeRepository
    {
        public BankTrxTypeRepository(HotDbContext context) : base(context) { }
        public async Task<List<BankTrxTypes>> ListBankTrxType()
        {
            return await GetAll().OrderBy(d => d.BankTrxType)
                .ToListAsync();
        }
        public async Task<BankTrxTypes?> GetBankTrxTypeById(byte bankTrxTypeId)
        {
            return await GetById(bankTrxTypeId);
        }
        public async Task<bool> AddBankTrxType(BankTrxTypes bankType)
        {
            await Create(bankType);
            await SaveChanges();
            return true;
        }
        public async Task<bool> DeleteBankTrxType(BankTrxTypes bankType)
        {
            Delete(bankType);
            await SaveChanges();
            return true;
        }
        public async Task<bool> UpdateBankTrxType(BankTrxTypes bankType)
        {
            Update(bankType);
            await SaveChanges();
            return true;
        }
    }
}
