using Hot4.DataModel.Models;
using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface IPaymentTypeRepository
    {
        Task<List<PaymentTypeModel>> ListPaymentType();
        Task AddPaymentType(PaymentTypes paymentType);
        Task UpdatePaymentType(PaymentTypes paymentType);
        Task DeletePaymentType(PaymentTypes paymentType);
    }
}
