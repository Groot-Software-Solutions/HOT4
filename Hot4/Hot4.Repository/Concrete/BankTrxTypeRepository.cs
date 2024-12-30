using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class BankTrxTypeRepository : RepositoryBase<BankTrxTypes>, IBankTrxTypeRepository
    {
        public BankTrxTypeRepository(HotDbContext context) : base(context) { }
        public async Task<List<BankTransactionTypeModel>> ListBankTrxType()
        {
            return await GetAll().OrderBy(d => d.BankTrxType)
                .Select(d => new BankTransactionTypeModel
                {
                    BankTrxTypeId = d.BankTrxTypeId,
                    BankTrxType = d.BankTrxType
                }).ToListAsync();
        }
        public async Task AddBankTrxType(BankTrxTypes bankType)
        {
            await Create(bankType);
            await SaveChanges();
        }

        public async Task DeleteBankTrxType(BankTrxTypes bankType)
        {
            await Delete(bankType);
            await SaveChanges();
        }
        public async Task UpdateBankTrxType(BankTrxTypes bankType)
        {
            await Update(bankType);
            await SaveChanges();
        }
    }
}
