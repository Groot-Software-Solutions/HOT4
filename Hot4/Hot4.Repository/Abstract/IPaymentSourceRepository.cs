using Hot4.DataModel.Models;
using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface IPaymentSourceRepository
    {
        Task<List<PaymentSourceModel>> ListPaymentSource();
        Task AddPaymentSource(PaymentSources paymentSource);
        Task UpdatePaymentSource(PaymentSources paymentSource);
        Task DeletePaymentSource(PaymentSources paymentSource);
    }
}
