using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetPaymentById(long paymentId);
        Task<bool> SaveUpdatePayment(Payment payment);
    }
}
