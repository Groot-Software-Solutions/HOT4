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

        public async Task AddPaymentSource(PaymentSources paymentSource)
        {
            await Create(paymentSource);
            await SaveChanges();
        }
        public async Task DeletePaymentSource(PaymentSources paymentSource)
        {
            Delete(paymentSource);
            await SaveChanges();
        }
        public async Task UpdatePaymentSource(PaymentSources paymentSource)
        {
            Update(paymentSource);
            await SaveChanges();
        }
        public async Task<List<PaymentSourceModel>> ListPaymentSource()
        {
            return await GetAll()
                .Select(d => new PaymentSourceModel
                {
                    PaymentSourceId = d.PaymentSourceId,
                    PaymentSource = d.PaymentSource,
                    PaymentSourceText = d.PaymentSource,
                    WalletTypeId = d.WalletTypeId,
                }).ToListAsync();
        }
    }
}
