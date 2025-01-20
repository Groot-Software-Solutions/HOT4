using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class PaymentSourceRepository : RepositoryBase<PaymentSources>, IPaymentSourceRepository
    {
        public PaymentSourceRepository(HotDbContext context) : base(context) { }

        public async Task<bool> AddPaymentSource(PaymentSources paymentSource)
        {
            await Create(paymentSource);
            await SaveChanges();
            return true;
        }
        public async Task<bool> DeletePaymentSource(PaymentSources paymentSource)
        {
            Delete(paymentSource);
            await SaveChanges();
            return true;
        }
        public async Task<bool> UpdatePaymentSource(PaymentSources paymentSource)
        {
            Update(paymentSource);
            await SaveChanges();
            return true;
        }
        public async Task<List<PaymentSources>> ListPaymentSource()
        {
            return await GetAll()
                .ToListAsync();
        }
        public async Task<PaymentSources> GetPaymentSourceById(byte PaymentSourceId)
        {
            var record = await GetById(PaymentSourceId);
            return record;
        }
    }
}
