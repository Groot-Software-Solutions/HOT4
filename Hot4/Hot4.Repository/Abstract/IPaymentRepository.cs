using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IPaymentRepository
    {
        Task<PaymentModel?> GetPaymentById(long paymentId);
        Task<List<PaymentModel>> GetPaymentByAccountId(long accountId, int pageNumber, int pageSize);
        Task<long?> SaveUpdatePayment(Payment payment);
    }
}
