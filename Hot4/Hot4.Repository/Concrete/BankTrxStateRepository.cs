using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class BankTrxStateRepository : RepositoryBase<BankTrxStates>, IBankTrxStateRepository
    {
        public BankTrxStateRepository(HotDbContext context) : base(context) { }

        public async Task<List<BankTrxStates>> ListBankTrxStates()
        {
            return await GetAll().OrderBy(d => d.BankTrxState)
               .ToListAsync();
        }
        public async Task<BankTrxStates?> GetBankTrxStateById(byte bankTrxStateId)
        {
            return await GetById(bankTrxStateId);
        }
        public async Task<bool> AddBankTrxState(BankTrxStates bankState)
        {
            await Create(bankState);
            await SaveChanges();
            return true;
        }
        public async Task<bool> DeleteBankTrxState(BankTrxStates bankState)
        {
            Delete(bankState);
            await SaveChanges();
            return true;
        }
        public async Task<bool> UpdateBankTrxState(BankTrxStates bankState)
        {
            Update(bankState);
            await SaveChanges();
            return true;
        }
    }
}
