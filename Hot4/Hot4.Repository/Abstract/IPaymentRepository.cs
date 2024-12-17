using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetPayment(long PaymentId);

    }
}
