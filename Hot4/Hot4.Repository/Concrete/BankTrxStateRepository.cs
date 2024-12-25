using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel.ApiModels;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class BankTrxStateRepository : RepositoryBase<BankTrxStates>, IBankTrxStateRepository
    {
        public BankTrxStateRepository(HotDbContext context) : base(context) { }

        public async Task<List<BankTransactionStateModel>> ListBankTrxStates()
        {
            return await GetAll().OrderBy(d => d.BankTrxState)
                .Select(d => new BankTransactionStateModel
                {
                    BankTrxStateId = d.BankTrxStateId,
                    BankTrxState = d.BankTrxState
                }).ToListAsync();
        }
        public async Task AddBankTrxState(BankTrxStates bankState)
        {
            await Create(bankState);
            await SaveChanges();
        }
        public async Task DeleteBankTrxState(BankTrxStates bankState)
        {
            await Delete(bankState);
            await SaveChanges();
        }
        public async Task UpdateBankTrxState(BankTrxStates bankState)
        {
            await Update(bankState);
            await SaveChanges();
        }
    }
}
