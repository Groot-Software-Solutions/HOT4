using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IPaymentRepository
    {
        Task<TblPayment?> GetPayment(long PaymentId);

    }
}
