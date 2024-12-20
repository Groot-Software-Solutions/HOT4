using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class BankRepository : RepositoryBase<Banks>, IBankRepository
    {
        public BankRepository(HotDbContext context) : base(context) { }
        public async Task<List<Banks>> ListBanks()
        {
            return await GetAll().OrderBy(d => d.Bank).ToListAsync();
        }
    }
}
