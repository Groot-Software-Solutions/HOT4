using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class PaymentSourceRepository : RepositoryBase<PaymentSources>, IPaymentSourceRepository
    {
        public PaymentSourceRepository(HotDbContext context) : base(context) { }
        public async Task<List<PaymentSources>> ListPaymentSource()
        {
            return await GetAll().ToListAsync();
        }
    }
}
