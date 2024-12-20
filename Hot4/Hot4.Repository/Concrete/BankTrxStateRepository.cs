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
            return await GetAll().OrderBy(d => d.BankTrxState).ToListAsync();
        }
    }
}
