using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IPaymentTypeRepository
    {
        Task<PaymentTypes> GetPaymentTypeById(byte PaymentTypeId);
        Task<List<PaymentTypes>> ListPaymentType();
        Task<bool> AddPaymentType(PaymentTypes paymentType);
        Task<bool> UpdatePaymentType(PaymentTypes paymentType);
        Task<bool> DeletePaymentType(PaymentTypes paymentType);
    }
}
