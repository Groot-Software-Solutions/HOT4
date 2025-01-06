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

        public async Task<List<BankModel>> ListBanks()
        {
            return await GetAll().OrderBy(d => d.Bank)
                .Select(d => new BankModel
                {
                    Bank = d.Bank,
                    BankId = d.BankId,
                    SageBankId = d.SageBankId
                }).ToListAsync();
        }
        public async Task UpdateBank(Banks bank)
        {
            Update(bank);
            await SaveChanges();
        }
        public async Task AddBank(Banks bank)
        {
            await Create(bank);
            await SaveChanges();
        }
        public async Task DeleteBank(Banks bank)
        {
            Delete(bank);
            await SaveChanges();
        }
    }
}
