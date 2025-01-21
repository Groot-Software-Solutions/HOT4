using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class BankRepository : RepositoryBase<Banks>, IBankRepository
    {
        public BankRepository(HotDbContext context) : base(context) { }
        public async Task<List<Banks>> ListBanks()
        {
            var records = await GetAll().OrderBy(d => d.Bank)
                .ToListAsync();
            return records;
        }
        public async Task<bool> UpdateBank(Banks banks)
        {
            Update(banks);
            await SaveChanges();
            return true;
        }
        public async Task<bool> AddBank(Banks banks)
        {
            await Create(banks);
            await SaveChanges();
            return true;
        }
        public async Task<bool> DeleteBank(Banks banks)
        {
            Delete(banks);
            await SaveChanges();
            return true;
        }
        public async Task<Banks?> GetByBankId(byte BankId)
        {
            return await GetById(BankId);
        }
    }
}
