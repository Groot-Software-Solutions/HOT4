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

        // need to  chcek ListPaymentSource()
        public async Task<List<PaymentSourceModel>> ListPaymentSource()
        {
            return await GetAll()
                .Select(d => new PaymentSourceModel
                {
                    PaymentSourceId = d.PaymentSourceId,
                    PaymentSource = d.PaymentSource,
                   // PaymentSourceText = d.PaymentSource,
                    WalletTypeId = d.WalletTypeId,
                }).ToListAsync();
        }
        public async Task<PaymentSources> GetPaymentSourceById(byte PaymentSourceId)
        {
            var record = await GetById(PaymentSourceId);
            return record;
        }
    }
}
