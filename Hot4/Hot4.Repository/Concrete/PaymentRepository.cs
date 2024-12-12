using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;

namespace Hot4.Repository.Concrete
{
    public class PaymentRepository : RepositoryBase<Payment>, IPaymentRepository
    {
        public PaymentRepository(HotDbContext context) : base(context) { }
        public async Task<Payment?> GetPayment(long PaymentId)
        {
            return await GetById(PaymentId);
        }

    }
}
