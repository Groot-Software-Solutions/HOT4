using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface IPaymentTypeRepository
    {
        Task<List<PaymentTypeModel>> ListPaymentType();
    }
}
