using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class PaymentSourceRepository : RepositoryBase<TblPaymentSource>, IPaymentSourceRepository
    {
        public PaymentSourceRepository(HotDbContext context) : base(context) { }
        public async Task<List<TblPaymentSource>> ListPaymentSource()
        {
            return await GetAll().ToListAsync();
        }
    }
}
