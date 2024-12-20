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
            return await GetAll().OrderBy(d => d.BankTrxType).ToListAsync();
        }
    }
}
