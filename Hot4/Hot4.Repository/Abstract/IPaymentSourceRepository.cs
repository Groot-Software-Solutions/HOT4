using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IPaymentSourceRepository
    {
        Task<PaymentSources> GetPaymentSourceById(byte PaymentSourceId);
        Task<List<PaymentSourceModel>> ListPaymentSource();
        Task<bool> AddPaymentSource(PaymentSources paymentSource);
        Task<bool> UpdatePaymentSource(PaymentSources paymentSource);
        Task<bool> DeletePaymentSource(PaymentSources paymentSource);
    }
}
