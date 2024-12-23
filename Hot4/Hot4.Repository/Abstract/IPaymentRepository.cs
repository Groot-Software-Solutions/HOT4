using Hot4.DataModel.Models;
using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface IPaymentRepository
    {
        Task<PaymentModel?> GetPayment(long paymentId);
        Task<List<PaymentModel>> ListPayment(long accountId, int pageNumber, int pageSize);
        Task<long?> SaveUpdatePayment(Payment payment);

    }
}
