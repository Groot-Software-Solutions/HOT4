using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class PaymentTypeRepository : RepositoryBase<PaymentTypes>, IPaymentTypeRepository
    {
        public PaymentTypeRepository(HotDbContext context) : base(context) { }
        public async Task<List<PaymentTypes>> ListPaymentType()
        {
            return await GetAll().ToListAsync();
        }
    }
}
