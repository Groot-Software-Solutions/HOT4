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
        public async Task<bool> AddPaymentType(PaymentTypes paymentType)
        {
            await Create(paymentType);
            await SaveChanges();
            return true;
        }
        public async Task<bool> DeletePaymentType(PaymentTypes paymentType)
        {
            Delete(paymentType);
            await SaveChanges();
            return true;
        }
        public async Task<bool> UpdatePaymentType(PaymentTypes paymentType)
        {
            Update(paymentType);
            await SaveChanges();
            return true;
        }
        public async Task<List<PaymentTypes>> ListPaymentType()
        {
            return await GetAll().ToListAsync();
        }
        public async Task<PaymentTypes?> GetPaymentTypeById(byte PaymentTypeId)
        {
            return await GetById(PaymentTypeId);
        }
    }
}
