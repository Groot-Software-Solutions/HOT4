using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class PaymentTypeRepository : RepositoryBase<PaymentTypes>, IPaymentTypeRepository
    {
        public PaymentTypeRepository(HotDbContext context) : base(context) { }

        public async Task AddPaymentType(PaymentTypes paymentType)
        {
            await Create(paymentType);
            await SaveChanges();
        }
        public async Task DeletePaymentType(PaymentTypes paymentType)
        {
            await Delete(paymentType);
            await SaveChanges();
        }
        public async Task UpdatePaymentType(PaymentTypes paymentType)
        {
            await Update(paymentType);
            await SaveChanges();
        }
        public async Task<List<PaymentTypeModel>> ListPaymentType()
        {
            return await GetAll()
                .Select(d => new PaymentTypeModel
                {
                    PaymentTypeId = d.PaymentTypeId,
                    PaymentType = d.PaymentType,
                    PaymentTypeText = d.PaymentType
                })
                .ToListAsync();
        }
    }
}
