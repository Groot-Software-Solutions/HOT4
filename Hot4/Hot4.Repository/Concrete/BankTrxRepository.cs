using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;

namespace Hot4.Repository.Concrete
{
    public class BankTrxRepository : RepositoryBase<BankTrx>, IBankTrxRepository
    {
        public BankTrxRepository(HotDbContext context) : base(context) { }

    }
}
